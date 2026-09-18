using RecrutAiDotnet.Configurations;
using recrutAiDotnet.Repositories;
using recrutAiDotnet.Models;
using recrutAiDotnet.Helpers;
using recrutAiDotnet.Enums;

using Stripe;
using Stripe.Checkout;

namespace recrutAiDotnet.Services;

public class StripeService
{
    private readonly EmpresaRepository _empresaRepository;
    private readonly UsuarioRepository _usuarioRepository;
    private readonly StripeClient _stripeClient;
    public StripeService(StripeClient stripeClient, UsuarioRepository usuarioRepository, EmpresaRepository empresaRepository)
    {
        _stripeClient = stripeClient;
        _usuarioRepository = usuarioRepository;
        _empresaRepository = empresaRepository;
    }

    public async Task<object> ContratarPlano(ContratarPlanoModel model, int usuarioId)
    {
        if (string.IsNullOrWhiteSpace(model.PaymentMethodId))
        {
            return new
            {
                statusCode = 400,
                details = new
                {
                    message = "PaymentMethodId é obrigatório."
                }
            };
        }

        var produto = await this.GetPlanoStripe(model.Plano);

        if (produto is null)
        {
            return new
            {
                statusCode = 404,
                details = new
                {
                    message = "Plano não encontrado."
                }
            };
        }

        var price = await this.GetPlanoPrice(produto);

        if (price is null)
        {
            return new
            {
                statusCode = 404,
                details = new
                {
                    message = "Preço do plano não encontrado."
                }
            };
        }

        var usuario = _usuarioRepository.GetUsuarioById(usuarioId);

        if (usuario is null)
        {
            return new
            {
                statusCode = 404,
                details = new
                {
                    message = "Usuario não encontrado."
                }
            };
        }
        var empresa = _empresaRepository.GetEmpresaById(usuario.EmpresaId);

        if (empresa is null)
        {
            return new
            {
                statusCode = 404,
                details = new
                {
                    message = "Empresa não encontrada."
                }
            };
        }

        var customerOptions = new CustomerCreateOptions
        {
            Email = usuario.Email,
            Name = empresa.RazaoSocial
        };

        var customer =
            await _stripeClient.V1.Customers.CreateAsync(
                customerOptions);

        var attachOptions = new PaymentMethodAttachOptions
        {
            Customer = customer.Id
        };

        var paymentMethod =
            await _stripeClient.V1.PaymentMethods.AttachAsync(
                model.PaymentMethodId,
                attachOptions
            );


        var subscriptionOptions = new SubscriptionCreateOptions
        {
            Customer = customer.Id,

            Items = new List<SubscriptionItemOptions>
        {
            new SubscriptionItemOptions
            {
                Price = price.Id
            }
        },

            DefaultPaymentMethod = paymentMethod.Id
        };

        var subscription =
            await _stripeClient.V1.Subscriptions.CreateAsync(
                subscriptionOptions);

        if (subscription.Status == "active")
        {
            _empresaRepository.UpdatePlano(
                empresa.Id,
                model.Plano
            );
        }

        return new
        {
            statusCode = 200,
            details = new
            {
                message = "Plano contratado.",
                customerId = customer.Id,
                subscriptionId = subscription.Id,
                status = subscription.Status
            }
        };
    }

    private async Task<Product?> GetPlanoStripe(PlanoEmpresa plano)
    {
        if (!StripePlansConfiguration.Products.TryGetValue(
                plano,
                out var produtoId))
        {
            return null;
        }

        return await _stripeClient.V1.Products.GetAsync(produtoId);
    }

    private async Task<Price?> GetPlanoPrice(Product produto)
    {
        var options = new PriceListOptions
        {
            Product = produto.Id,
            Active = true,
            Type = "recurring"
        };

        var prices = await _stripeClient.V1.Prices.ListAsync(options);

        return prices.Data.FirstOrDefault();
    }

    public async Task<object> CadastrarCartaoTeste()
    {
        var options = new PaymentMethodCreateOptions
        {
            Type = "card",
            Card = new PaymentMethodCardOptions
            {
                Token = "tok_visa"
            }
        };

        var paymentMethod =
            await _stripeClient.V1.PaymentMethods.CreateAsync(options);

        return new
        {
            paymentMethodId = paymentMethod.Id,
            tipo = paymentMethod.Type
        };
    }
}