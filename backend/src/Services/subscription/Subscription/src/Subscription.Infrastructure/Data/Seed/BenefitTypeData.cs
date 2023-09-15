using Microsoft.Extensions.Hosting;
using Subscription.Application.Interfaces.Repositories.Base;

namespace Subscription.Infrastructure.Data.Seed
{
    public class BenefitTypeData : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        public BenefitTypeData(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using (var scope = _serviceProvider.CreateAsyncScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                await context.Database.EnsureCreatedAsync(cancellationToken);

                var unittypeRepo = scope.ServiceProvider.GetRequiredService<IBaseRepositoryAsync<BenefitType, string>>();
                var UnitTypes = new List<BenefitType>()
                {
                    new BenefitType{ Id = "abf05e97-f807-43f1-9552-c787c5a31d39", Name = "Basic indDeals", Active = true, Size = "S", ShowInUnitItem = true, ShowInSubscribe = false },
                    new BenefitType{ Id = "1b44d103-10c4-4558-9331-09c9f0dbe73f", Name = "Premium inDeals", Active = true, Size = "M", ShowInUnitItem = true, ShowInSubscribe = false },
                    new BenefitType{ Id = "12efc120-73a1-4f12-8ae5-1b8afe991a29", Name = "Superior inDeals", Active = true, Size = "L", ShowInUnitItem = true, ShowInSubscribe = false },
                    new BenefitType{ Id = "a1dfc19b-4d38-4c3f-b57b-14ee92c58620", Name = "Basic inSpiration", Active = true, Size = "S" , ShowInUnitItem = true, ShowInSubscribe = false},
                    new BenefitType{ Id = "21f0901d-a2c3-47ca-bcaf-b0195ed28af1", Name = "Premium inSpiration", Active = true, Size = "M", ShowInUnitItem = true, ShowInSubscribe = false },
                    new BenefitType{ Id = "bac65b53-6d48-4e69-af38-2ea6b2d5e6e0", Name = "Superior inSpiration", Active = true, Size = "L", ShowInUnitItem = true, ShowInSubscribe = false },
                    new BenefitType{ Id = "80e9938f-ec6d-4938-83f5-e8125e85c492", Name = "HotDeals Of The Week", Active = true, Size = "" , ShowInUnitItem = true, ShowInSubscribe = false},
                    new BenefitType{ Id = "104d6d15-21ce-4404-8622-c6927cee99c6", Name = "HotDeals you may like", Active = true, Size = "", ShowInUnitItem = true, ShowInSubscribe = false },
                    new BenefitType{ Id = "9fcf1fdf-641c-41cb-90d3-2db538b7d94a", Name = "inMerchants you may like", Active = true, Size = "", ShowInUnitItem = true, ShowInSubscribe = false },
                    new BenefitType{ Id = "6b07115a-8566-48e9-9a65-183476911193", Name = "Recomended merchants", Active = true, Size = "", ShowInUnitItem = true, ShowInSubscribe = true },
                    new BenefitType{ Id = "35b1fee8-f356-4eb4-89bc-9bf99516b588", Name = "Highlight hotdeals", Active = true, Size = "", ShowInUnitItem = false, ShowInSubscribe = true },
                    new BenefitType{ Id = "636bafb2-61b5-40d5-9f56-a30b864ed071", Name = "Gold crown", Active = true, Size = "", ShowInUnitItem = false, ShowInSubscribe = true },
                    new BenefitType{ Id = "6855cb23-e774-458c-a163-0e9ddb5c7725", Name = "Up Product indeals", Active = true, Size = "", ShowInUnitItem = false, ShowInSubscribe = true },
                    new BenefitType{ Id = "4272111d-f267-4503-9e2b-77cf6ebe77bb", Name = "Product Discount", Active = true, Size = "", ShowInUnitItem = false, ShowInSubscribe = true },
                    new BenefitType{ Id = "5fdbdea7-63c2-41f2-99c4-aad729fd4c33", Name = "inhits today maksimal 1 hari 1 berita", Active = true, Size = "", ShowInUnitItem = false, ShowInSubscribe = true },
                    new BenefitType{ Id = "1587c5d6-3070-4709-b610-b6a6bdab6de5", Name = "Consultation with tessa", Active = true, Size = "", ShowInUnitItem = false, ShowInSubscribe = true },
                };
                foreach (var unitType in UnitTypes)
                {
                    if (!await unittypeRepo.NoTrackingEntities.AnyAsync(c => c.Name.ToLower() == unitType.Name.ToLower(), cancellationToken: cancellationToken))
                    {
                        var res = await unittypeRepo.AddAsync(unitType, cancellationToken);
                    }
                }

                var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                if (environment == Environments.Development)
                {
                    var unitItems = new List<UnitItem>()
                {
                    new UnitItem
                    {   Id = "a7a8917a-46f4-4bef-8796-3f9f6d978e65",
                        BenefitType = "Basic indDeals", BenefitSize ="S", Title = "Basic InDeals", Active = true,
                        LiveDuration = 30, ValidDuration = 60, Price = 100000, Priority = 1, QuantityUpload = 5,
                        ShowInPackage = true, ShowInPageInDealPrice = true, ShowInPageInSpirationPrice = false,
                        DiscountPercent = 0,DiscountPrice = 0,CreatedBy = "System", Description=""
                    },
                    new UnitItem
                    {   Id = "4bbb17b3-d281-449d-9825-06160b886cc7",
                        BenefitType = "Premium inDeals", BenefitSize ="M", Title = "Premium inDeals", Active = true,
                        LiveDuration = 30, ValidDuration = 90, Price = 100000, Priority = 2, QuantityUpload = 15,
                        ShowInPackage = true, ShowInPageInDealPrice = true, ShowInPageInSpirationPrice = false,
                        DiscountPercent = 0,DiscountPrice = 0, CreatedBy = "System", Description="",
                    },
                    new UnitItem
                    {   Id = "85b31f67-4e07-4a69-9a66-7f064424b408",
                        BenefitType = "Superior inDeals", BenefitSize ="M", Title = "Superior inDeals", Active = true,
                        LiveDuration = 30, ValidDuration = 90, Price = 100000, Priority = 3, QuantityUpload = 25,
                        ShowInPackage = true, ShowInPageInDealPrice = true, ShowInPageInSpirationPrice = false,
                        DiscountPercent = 0,DiscountPrice = 0, CreatedBy = "System", Description="",
                    },
                    new UnitItem
                    {   Id = "580b7d08-659d-479c-9b79-1c3f2d2e4316",
                        BenefitType = "Basic inSpiration", BenefitSize ="S", Title = "Basic inSpiration", Active = true,
                        LiveDuration = 30, ValidDuration = 30, Price = 25000, Priority = 1, QuantityUpload = 1,
                        ShowInPackage = true, ShowInPageInDealPrice = false, ShowInPageInSpirationPrice = true,
                        DiscountPercent = 0,DiscountPrice = 0, CreatedBy = "System", Description="",
                    },
                    new UnitItem
                    {   Id = "e764151a-ce99-47ed-b74a-b8bfa67feaa8",
                        BenefitType = "Recomended merchants", BenefitSize ="", Title = "Recomended Merchants", Active = true,
                        LiveDuration = 0, ValidDuration = 0, Price = 0, Priority = 2, QuantityUpload = 0,
                        ShowInPackage = true, ShowInPageInDealPrice = false, ShowInPageInSpirationPrice = false,
                        DiscountPercent = 0,DiscountPrice = 0, CreatedBy = "System", Description="",
                    },
                };
                    var unitItemRepo = scope.ServiceProvider.GetRequiredService<IBaseRepositoryAsync<UnitItem, string>>();
                    foreach (var unitItem in unitItems)
                    {
                        if (!await unitItemRepo.NoTrackingEntities.AnyAsync(c => c.Title.ToLower() == unitItem.Title.ToLower(), cancellationToken: cancellationToken))
                        {
                            var res = await unitItemRepo.AddAsync(unitItem, cancellationToken);
                        }
                    }

                    var packageCrystal = new Package
                    {
                        Title = "Crystal",
                        Description = "Crystal",
                        Duration = 365,
                        Price = 1000000,
                        DiscountPrice = 0,
                        DiscountPercent = 0,
                        Priority = 1,
                        ImageUrl = "",
                        Active = true,
                        CreatedBy = "System",
                    };
                    packageCrystal.PackageDetails = new List<PackageDetail>()
                            {
                                new PackageDetail
                                {
                                    PackageId = packageCrystal.Id,
                                    UnitItemId = "a7a8917a-46f4-4bef-8796-3f9f6d978e65",
                                    ImageUrl = "",
                                    Quantity = 80,
                                },
                                new PackageDetail
                                {
                                    PackageId = packageCrystal.Id,
                                    UnitItemId = "4bbb17b3-d281-449d-9825-06160b886cc7",
                                    ImageUrl = "",
                                    Quantity = 10,
                                },
                                new PackageDetail
                                {
                                    PackageId = packageCrystal.Id,
                                    UnitItemId = "85b31f67-4e07-4a69-9a66-7f064424b408",
                                    ImageUrl = "",
                                    Quantity = 5,
                                },
                                new PackageDetail
                                {
                                    PackageId = packageCrystal.Id,
                                    UnitItemId = "580b7d08-659d-479c-9b79-1c3f2d2e4316",
                                    ImageUrl = "",
                                    Quantity = 10,
                                }
                            };
                    var packageRuby = new Package
                    {
                        Title = "Ruby",
                        Description = "Ruby",
                        Duration = 365,
                        Price = 1300000,
                        DiscountPrice = 0,
                        DiscountPercent = 0,
                        Priority = 2,
                        ImageUrl = "",
                        Active = true,
                        CreatedBy = "System",
                    };
                    packageRuby.PackageDetails = new List<PackageDetail>()
                            {
                                new PackageDetail
                                {
                                    PackageId = packageRuby.Id,
                                    UnitItemId = "a7a8917a-46f4-4bef-8796-3f9f6d978e65",
                                    ImageUrl = "",
                                    Quantity = 150,
                                },
                                new PackageDetail
                                {
                                    PackageId = packageRuby.Id,
                                    UnitItemId = "4bbb17b3-d281-449d-9825-06160b886cc7",
                                    ImageUrl = "",
                                    Quantity = 50,
                                },
                                 new PackageDetail
                                {
                                    PackageId = packageRuby.Id,
                                    UnitItemId = "85b31f67-4e07-4a69-9a66-7f064424b408",
                                    ImageUrl = "",
                                    Quantity = 20,
                                },
                                new PackageDetail
                                {
                                    PackageId = packageRuby.Id,
                                    UnitItemId = "580b7d08-659d-479c-9b79-1c3f2d2e4316",
                                    ImageUrl = "",
                                    Quantity = 20,
                                },
                               new PackageDetail
                                {
                                    PackageId = packageRuby.Id,
                                    UnitItemId = "e764151a-ce99-47ed-b74a-b8bfa67feaa8",
                                    ImageUrl = "",
                                    Quantity = 1,
                                }
                            };
                    var package = new Package
                    {
                        Title = "Diamond",
                        Description = "Diamond",
                        Duration = 365,
                        Price = 1500000,
                        DiscountPrice = 0,
                        DiscountPercent = 0,
                        Priority = 3,
                        ImageUrl = "",
                        Active = true,
                        CreatedBy = "System",
                    };
                    package.PackageDetails = new List<PackageDetail>()
                            {
                                new PackageDetail
                                {
                                    PackageId = package.Id,
                                    UnitItemId = "a7a8917a-46f4-4bef-8796-3f9f6d978e65",
                                    ImageUrl = "",
                                    Quantity = 250,
                                },
                                new PackageDetail
                                {
                                    PackageId = package.Id,
                                    UnitItemId = "4bbb17b3-d281-449d-9825-06160b886cc7",
                                    ImageUrl = "",
                                    Quantity = 150,
                                },
                                 new PackageDetail
                                {
                                    PackageId = package.Id,
                                    UnitItemId = "85b31f67-4e07-4a69-9a66-7f064424b408",
                                    ImageUrl = "",
                                    Quantity = 100,
                                },
                                new PackageDetail
                                {
                                    PackageId = package.Id,
                                    UnitItemId = "580b7d08-659d-479c-9b79-1c3f2d2e4316",
                                    ImageUrl = "",
                                    Quantity = 50,
                                },
                               new PackageDetail
                                {
                                    PackageId = package.Id,
                                    UnitItemId = "e764151a-ce99-47ed-b74a-b8bfa67feaa8",
                                    ImageUrl = "",
                                    Quantity = 1,
                                }
                            };

                    var packages = new List<Package>()
                    {
                        packageCrystal, packageRuby, package
                    };

                    var packageRepo = scope.ServiceProvider.GetRequiredService<IBaseRepositoryAsync<Package, string>>();
                    foreach (var packageItem in packages)
                    {
                        if (!await packageRepo.NoTrackingEntities.AnyAsync(c => c.Title.ToLower() == packageItem.Title.ToLower(), cancellationToken: cancellationToken))
                        {
                            var res = await packageRepo.AddAsync(packageItem, cancellationToken);
                        }
                    }
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
