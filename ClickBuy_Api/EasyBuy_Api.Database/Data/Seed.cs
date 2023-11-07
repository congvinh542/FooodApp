using ClickBuy_Api.Database.Entities;
using ClickBuy_Api.Database.Entities.Catalog;
using ClickBuy_Api.Database.Entities.Products;
using ClickBuy_Api.Database.Entities.System;
using ClickBuy_Api.Database.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ClickBuy_Api.Database.Data
{
    public class Seed
	{
		public async static Task<int> SeedUser(UserManager<User> userManager, RoleManager<Role> roleManager, ClickBuyDbContext context)
		{
			if (userManager.Users.Any())
			{
				return 0;
			}
			string path = Directory.GetCurrentDirectory() + "\\Resoucres\\";

			if (!File.Exists(path + "UserSeedData.json"))
			{
				CreateUser(userManager, roleManager, context);
			}
			if (!File.Exists(path + "PermissionSeedData.json"))
			{
				CreatePermission(userManager, roleManager, context);
			}

			var users = await File.ReadAllTextAsync(path + "UserSeedData.json");
			var usersToSeed = System.Text.Json.JsonSerializer.Deserialize<List<User>>(users);
			var roles = new List<Role>
			{
				new Role(){ Name=EUserRoles.Admin.ToString()},
				new Role(){ Name=EUserRoles.Member.ToString()},
				new Role(){ Name=EUserRoles.Mod.ToString()},
				new Role(){ Name=EUserRoles.Author.ToString()},

			};

			// add permission
			var permissions = await File.ReadAllTextAsync(path + "PermissionSeedData.json");
			var permissionsToSeed = System.Text.Json.JsonSerializer.Deserialize<List<Permission>>(permissions);
			foreach (var permission in permissionsToSeed)
			{
				await context.Permissions.AddAsync(permission);
			}
			await context.SaveChangesAsync();

			var permissionNew = await context.Permissions.ToListAsync();


			foreach (var role in roles)
			{
				await roleManager.CreateAsync(role);

				// add permission to role admin
				if (role.Name.Equals(EUserRoles.Admin.ToString()))
				{
					context.RolePermissions.AddRange(permissionNew.Select(x => new RolePermission()
					{
						RoleId = role.Id,
						PermissionId = x.Id.Value,
						IsActive = true,
						CreatedAt = DateTime.Now,
					}));
				}

			}
			await context.SaveChangesAsync();


            // add user
            foreach (var user in usersToSeed)
			{
				user.ResetCode = "";
				user.Otp = "";
				user.UserName = user.FirstName + user.LastName;
				user.FirstName = "Đặng Công";
				user.LastName = "Vinh";
				user.CreatedAt = DateTime.Now;
				user.IsActive = true;
				user.CreatedBy = "System";
				user.Phone = "0963562615";
				user.Address = "Khánh Hòa";
				user.DateOfBirth = new DateTime(1999, 06, 13);
				user.ImageId = null;
				user.TinhId = null;
				user.HuyenId = null;
				user.XaId = null;
				user.GenderId = null;
				await userManager.CreateAsync(user, "Abc12345@");

				if (user.UserName.Equals(EUserRoles.Admin.ToString().ToLower()))
				{
					await userManager.AddToRoleAsync(user, EUserRoles.Admin.ToString());
					await userManager.AddToRoleAsync(user, EUserRoles.Mod.ToString());

				}
				else if (user.UserName.Equals(EUserRoles.Mod.ToString().ToLower()))
				{
					await userManager.AddToRoleAsync(user, EUserRoles.Mod.ToString());
				}
				else if (user.UserName.Equals(EUserRoles.Author.ToString().ToLower()))
				{
					await userManager.AddToRoleAsync(user, EUserRoles.Author.ToString());
				}
				else
				{
					await userManager.AddToRoleAsync(user, EUserRoles.Member.ToString());
				}

			};
			return usersToSeed.Count;
		}

		private static void CreatePermission(UserManager<User> userManager, RoleManager<Role> roleManager, ClickBuyDbContext context)
		{
			var permission = new List<Permission>
			{
                // Permission
                new Permission(){ Key="ReadPermission", Description="Read Permission",Value="READ"},
				new Permission(){ Key="WritePermission", Description="Write Permission",Value="WRITE"},
                // User
                new Permission(){ Key="UserRead", Description="User Read",Value="READ"},
				new Permission(){ Key="UserWrite", Description="User Write",Value="WRITE"},
                // Role
                new Permission(){ Key="RoleRead", Description="Role Read",Value="READ"},
				new Permission(){ Key="RoleWrite", Description="Role Write",Value="WRITE"},
                // Notification
                new Permission(){ Key="NotificationRead", Description="Notification Read",Value="READ"},
				new Permission(){ Key="NotificationWrite", Description="Notification Write",Value="WRITE"},
			};
			// Save Permission to FILE
			var path = Directory.GetCurrentDirectory() + "\\Resoucres\\";
			var permissions = System.Text.Json.JsonSerializer.Serialize(permission);
			File.WriteAllText(path + "PermissionSeedData.json", permissions);

		}

        public async static Task<int> SeedImage(ClickBuyDbContext context)
        {
            if (context.Imagess.Any())
            {
                return 0;
            }

            var imageCF = new Guid("677767EE-F23E-44F4-BF45-315FFB52F088");
            var imageST = new Guid("8CE58AEB-0418-4B55-9A1D-B3B66CFD9FA8");
            var imageTS = new Guid("D2CDFE10-2A15-4A6E-9993-60E32A715840");
            var imageMA = new Guid("AD2D1D78-F34C-4680-9EF6-73AC6FB3A6DE");
            var imageNN = new Guid("86B4BC97-9757-4B08-B385-26F44AACDFC4");

            var images = new List<Images>
				{
					new Images(){ Id = imageCF, FileName = "Cà phê", FilePath="https://lh3.googleusercontent.com/u/3/drive-viewer/AK7aPaBV2YeyUUkwgDT_8qGv_QJvVF8MpwkmGuh5PtrAsq3wNm5c4g7J-KIo3fK42tAXTbQ1_Lx6wIo-o2fz4Eq0Q1UbBTSrPg=w1920-h897"},
					new Images(){ Id = imageST, FileName = "Sinh tố",  FilePath="https://lh3.googleusercontent.com/fife/AK0iWDxLbh0V1bjL3IDWgLEVFpOOHhiSJy1fHVjYZX-61x_kWLrZlFGuXs-Pp9u0B4NlV0MF5Ly0zRwGP9ABT_qqFBv62VLR_OX6epB9tSeVY0bJ9kEeBpilwGrJBlsAtMdX-GrrUkd_-N3AY_UTjAfbZR5JBLemMP-TC0_mSUNuy1wQ7XCsECA7b_COxYZLSXEvNgojO72VyuTdA7F8DTOl5PjqVpk_pmRctxLEZrih57T6LtckYxrIx0sd4I25UZlbtwNnOu7hx9LcFzxUTABRhI7-z4HiiUouyW8cs3thkXPxS8HiHICaEvi0DDiM158MLCmhbLRe4oMKzSNnB13HCWlvpZ7H9OgVh1wIHUmYHg1pwxd1bgMZeaCYBFB6s7JH5QWglapG3AmCTDXQ1QG4C5W2Z9vuUJimUjKRQGrrpx6S93WG8Ml8xyaPzphBxGK1crhUnPbRnsw2dC9jhV8BESZPZLUaDc7ECb20C7qVnBxEQu5k5NRufB_r5WoXDOf2FF-BcRifC5GvFLryPxtKya7Cvnkkc-v4nKXpVfv4H8QMa00mYcG0B1LEbPCTwxvqV_06QlKseizs6WDiUGJAe1DTfSjPNJNfyKoNHftFXC6OWL0KCfxO2Fc2TFb8JcK292JJxV8Fz9049X6ujufiF6vE7QUIN8mZs9VH55wIRRzIRtvkwrTRllA_XqdVVNTcm78wH731T8mZmC4TnSHOJNeOTaFnV-LAskhXj88DN6xwFso2DEP-mQlGAH9NP7zYEH5rCOgUXFcLqSD9zUPnBFuA-abOnabQg63Hc9XbijpHO2eTn-UnQB7XuNUC-NYePPkkUBLXmmsZiAIWOJR6K-OqNvxxDXCtKhoiViguN1smi4DZ8AluP-UjK3KmcsognKY6FF9t61wrEGH6z5a9cucS4I3X0kftvGwqjTxDrBsP6FfgLXmp9x1wUAJlDdCyxOxV1Onk8qcexH31UluTs-kgK9CQUxWeK0WVyvkizOHbjzCFyd-d56r573ibdOgVVx1dDPzsX1DQpwVis3sQLY6mbCrJABzP6WfVHNibLdpY49GfOiFpCLI2Orko9r2UrDC6O4YRQUAkeAysGbsXtBbwCqLTbtikXF8mjj4MtSFGu5zg_VblEvjtCNSUq00Ily6y_Km06hrIgkMRHbzAUGDLYCZm6xabDsWL2qOSbhWH9lf0TqsMtFJu_2S3eMOx-CaLIQfPMSuoFzMAyTBkuS4fT11GHah9iBcCFqWdAVMcFPkBnVc-vBkwlK6t0hAYfbr6ICchMwTj24Zowf-QV1sZDwcvWCeEONjHnwGCqVq-qTZpJHMfrljCiSmS8yZH9mWgWJNfY11m7rM_fwGfUTZC_wpy3afV4xDJJZvYa0ms7J7Q9rIwwizQVYCjd-LnEIki3TVjtE00hI-H_Yicu09dBJHJipcNd_ayEgBMYT7Hzj_jMWMJjErwjkg5WyJpi4ZQLNTM68evcLgm1ZXPHQz1Oj_PempBEehVmb_YO6bo-xWz3dAv8yzxrQqMdStp3sLd1giYXXBLCHIplp5SH3aG4ahRlpVeZRDb2Sr7rJk-_4f3=w1920-h897"},
					new Images(){ Id = imageTS, FileName = "Trà sữa", FilePath="https://lh3.googleusercontent.com/fife/AK0iWDyEED_a-rgVGnrVtttbkyGHT3VBeRTtGb4TbIopNOnaqDB-mkYb6quBe3voyNb5KtG-jDCJ3z_CQ5YjHKlJ3ACgK32kJK69qYj28OIrkoK8vgGAXJIZNfvKXnkflMk8nuUZhEWLYdoO9vHdcTy_bt_SylfcGOVAMObIqTS5T9TtEC8mFNVkZWu-7rH081zYATTOtUgmj7GjpAVMQzROFsfvgli44giQPAa5-ZbfEkxd2S2JwEIBqnP8r3if035HW2nflt5rbFVzdI46Exxxyq1G8Oe1nY-M9xG3JzJyzSonKXc1ue1LXSr20HM0H_0zV9is91in1e1s9FHq9VsXQg5bfyyAgN_gWYvQiOLnfL_flFrZmBNqn0JOZEmbGQFEllSOT4B3u93M_yiiqEvoUU0Bg4wIMm135XiNupEeZCXigSKNzcjTha_ouQhkp1SOYPBW_CaAWAxqSb7e5TZ992EpEuq-_MncNylE5ObecG_Hn5oEUJL5B4YDdZveTfWJInsVTkJATRYRhAts-ZQvKurutqwxHAVUTG65YmY5dNFiFC_Y10pebYx7XanLN-PPqFVD3zxNcKMJli1oqV5COqB8-l5ae4K7xdnG3Cp8n6ZE442-I0yYo007rmz_p6cW0AOnWzb63fMVwpplxzRBy3sRiinOJeeSjhTSYlePMYCB9Y11hQNQpEO2vUg4u8uBxrdBEgCTv7WMj8hv3nUfCWol9hQ6jgyvz8WJByhzAQ7myITikMDX6HCpTjDUvwJXNGdQFXxx3DQwKbKgcoLuovkX3RLSpD51vw6z863Zi0F0OkCpZFT7rXsoub8XnWNRjS5VtM1wydYMfKeRTRDWAgBN5j4Q9739ChsHYhCx7bsSyAh-qDBXB_wP4rwsJXE9c4MfbyUzuAmVm1vZr5D9V1rMLzdxsIhQfDbIMWw5q4XGGbIwYhcflOariTGQU0hEVoC7SgtkqImAQTE_DtvWApmf0F-UZ-bZ8e9TjzipvSefYls0khxrcGts4xZVcm9c-qAbJqLsIatBbYpNonWHXTVXSgY492-4DS9-Zq1fu0ZnU5DgQ73X8_ONAfM_RFfMSU0TMLY19dr9-m8nN0c_v9E7iRsdwr4i1m8-b9P3LKGg-63mIfJYQ_xiovQqdYbrCyjEC11o-L2ITjUUo31jzFopma3YsVbhis9bS2l3Avn0QeZe_flvNNdzV5PjlKO29kKFzJLIEt1YagLIpKNaFi8SXdpVqSWjTDcPw_LCppGYiWLaUYfgA-8D7V79smFEnGp5dSgkGOleFNb88bSCwWRBwcYCqhSaJdckirFvveX0fu0GzJhgVxPT0fG0Q73XghRgvNUjL2SKLT5ENh7MTxFd6naPZPU1km83UlzHXO-TIPB1cnkRDY9iE0LceZshuTwvQXGglGIkUdaVYulaaXM_8wG9Z6Nvj4GmhQOzIXY03UrVQWwDChu5VfSzdtOzcWOuIYsGLVt1BHaO1yBCpkV_GhZVtV4j2mSCuR_QcT_MWFmMmkH-J6mCKCa2wEzuMldVTt8bK8dRQuDCB_nOkKmSym1w-qNZJJJhF3XjBg-WoHRN=w1920-h897"},
					new Images(){ Id = imageMA, FileName = "Món ăn", FilePath="https://lh3.googleusercontent.com/u/3/drive-viewer/AK7aPaDGHaylFp4jbvs-9BG3smqcY2BHdi8v2TtlqomfGpwBORAhl-t5BKhTFZzf_eXKzuo4HsQfxgqiu_7Fw30gf5m97EFJiQ=w1920-h897"},
					new Images(){ Id = imageNN, FileName = "Nước ngọt", FilePath="https://lh3.googleusercontent.com/u/3/drive-viewer/AK7aPaBlg3f67Fg6N_DWc3gSKFhM265oguNJaeJsTf4kttIqmfs284WHFdd3cEbS5ggT3p8yp78WuYkdUzHNTj_FMK9hAqGdVw=w1920-h897"},
				};

            context.Imagess.AddRange(images);
            await context.SaveChangesAsync();

            return images.Count;
        }

        public async static Task<int> SeedCategory(ClickBuyDbContext context)
        {
            if (context.Categories.Any())
            {
                return 0;
            }

            var categoriesCF = new Guid("677767EE-F23E-44F4-BF45-31ACFB52F088");
            var categoriesST = new Guid("467BEC5E-5A4C-4E5A-BB54-7DF0331B1ECC");
            var categoriesTS = new Guid("D092145B-C8D9-476A-B454-3F5D88848781");
            var categoriesMA = new Guid("AA63E04F-ABD6-407B-9F4E-FD90C264DFE1");
            var categoriesNN = new Guid("DBF246E6-23B6-46FC-8346-5741F064FC87");
            var cofe = new Guid("677767EE-F23E-44F4-BF45-315FFB52F088");
            var sinhto = new Guid("8CE58AEB-0418-4B55-9A1D-B3B66CFD9FA8");
            var trasua = new Guid("D2CDFE10-2A15-4A6E-9993-60E32A715840");
            var food = new Guid("AD2D1D78-F34C-4680-9EF6-73AC6FB3A6DE");
            var nuocngot = new Guid("86B4BC97-9757-4B08-B385-26F44AACDFC4");

            var categories = new List<Category>
            {
                new Category(){ Id = categoriesCF, Code = "Drink", Name = "Coffee", ImageId = cofe},
                new Category(){ Id = categoriesST, Code = "Drink", Name = "Sinh tố", ImageId = sinhto},
                new Category(){ Id = categoriesTS, Code = "Drink", Name = "Trà sữa", ImageId = trasua},
                new Category(){ Id = categoriesMA, Code = "Food", Name = "Món ăn", ImageId = food},
                new Category(){ Id = categoriesNN, Code = "Drink", Name = "Nước ngọt", ImageId = nuocngot},
            };

            context.Categories.AddRange(categories);
            await context.SaveChangesAsync();

            return categories.Count;
        }

        public async static Task<int> SeedProduct(ClickBuyDbContext context)
        {
            if (context.Products.Any())
            {
                return 0;
            }

            var imageCF = new Guid("677767EE-F23E-44F4-BF45-315FFB52F088");

            var categoriesCF = new Guid("677767EE-F23E-44F4-BF45-31ACFB52F088");
            var categoriesST = new Guid("467BEC5E-5A4C-4E5A-BB54-7DF0331B1ECC");
            var categoriesTS = new Guid("D092145B-C8D9-476A-B454-3F5D88848781");
            var categoriesMA = new Guid("AA63E04F-ABD6-407B-9F4E-FD90C264DFE1");
            var categoriesNN = new Guid("DBF246E6-23B6-46FC-8346-5741F064FC87");

			var cfDen = new Guid("E21B7DD2-5BEF-4413-8588-F3320C20F151");
			var cfSua = new Guid("7F092EE4-0243-4AD8-B5F4-EF46F66FE8AC");
			var bx = new Guid("8437B8A4-D690-4CA7-A5CD-F89364AF1DB7");
			var bxs = new Guid("8437B8A4-D690-4CA7-ACAD-F89364AF1DB7");

            var products = new List<Product>
            {
                new Product(){ Id = cfDen, Name = "Cà phê đen", CategoryId = categoriesCF, Quantity = 100, Price = 15000, IsHot = true, ImageId = imageCF},
                new Product(){ Id = cfSua, Name = "Cà phê sữa", CategoryId = categoriesST, Quantity = 100, Price = 18000, IsHot = true,  ImageId = imageCF},
                new Product(){ Id = bx, Name = "Bạc xỉu", CategoryId = categoriesMA, Quantity = 100, Price = 22000, IsHot = true, ImageId = imageCF},
                new Product(){ Id = bxs, Name = "Bạc Test", CategoryId = categoriesNN, Quantity = 100, Price = 22000, IsHot = true, ImageId = imageCF},
            };

            context.Products.AddRange(products);
            await context.SaveChangesAsync();

            return products.Count;
        }

        public async static Task<int> SeedTinh(ClickBuyDbContext context)
		{
            if (context.Tinhs.Any())
            {
                return 0;
            }

            var dakLak = new Guid("677767DD-F23E-44F4-BF45-315FFB52F088");
			var lamDong = new Guid("594DFCEA-DE45-4540-A2F4-146F628C726A");
			var binhDinh = new Guid("0215C0A2-44DF-4FBF-9996-45CFE803941A"); 
			var phuYen = new Guid("4ABE246D-741D-40DB-A0BF-816A266FEF04");
			var khanhHoa = new Guid("76BDF3EF-DC2C-46EB-A65C-68B14D932D80");

			var tinhs = new List<Tinh>
			{
				new Tinh(){ Id = dakLak, Code="47", Description = "ĐL", Name = "Đắk Lắk"},
				new Tinh(){ Id = lamDong, Code = "49", Description = "LĐ", Name = "Lâm Đồng"},
				new Tinh(){ Id = binhDinh, Code = "77", Description = "BD", Name = "Bình Định"},
				new Tinh(){ Id = phuYen, Code = "78", Description = "PY", Name = "Phú Yên"},
				new Tinh(){ Id = khanhHoa,  Code = "79", Description = "KH", Name = "Khánh Hòa"},
			};

			context.Tinhs.AddRange(tinhs);
			await context.SaveChangesAsync();

			return tinhs.Count;
		}

		public async static Task<int> SeedGender(ClickBuyDbContext context)
		{
			if (context.Genders.Any())
			{
				return 0;
			}
			var male = new Guid("0A42ABFA-FCAF-4BA0-8BEC-2331476888BF");
            var female = new Guid("0A42ABFA-FCAF-4BA0-8BEC-2331476888DF");

			var genders = new List<Gender>
			{
				new Gender(){ Id = male, Code = "M", Description = "Nam", Name = "Nam"},
				new Gender(){ Id = female, Code = "F", Description = "Nữ", Name = "Nữ"}
			};
			context.Genders.AddRange(genders);
			await context.SaveChangesAsync();
			return genders.Count;
        }

        public async static Task<int> SeedHuyen(ClickBuyDbContext context)
		{
			#region TinhId
			var phuYen = new Guid("4ABE246D-741D-40DB-A0BF-816A266FEF04"); 
			var khanhHoa = new Guid("76BDF3EF-DC2C-46EB-A65C-68B14D932D80");
			#endregion

			#region HuyenId
			var songCau = new Guid("223002AC-0E1B-4B93-A2B0-FAFBB6C98A05"); 
			var tuyAn = new Guid("F7734EB0-9C9E-411D-9794-89CDE95C69F2"); 
			var tuyHoa = new Guid("9DEAE3A4-80B4-40DD-84FA-EB2EDCB5503B"); 

			var camRanh = new Guid("0D837345-1437-4BCF-92CE-2AF554108800"); 
			var nhaTrang = new Guid("C473F5DB-F805-470F-90EF-65375B5FBB29"); 
			var ninhHoa = new Guid("DD264027-9B40-42BD-8765-82143AE7B424"); 
			var camLam = new Guid("11C04D5E-7990-4376-A43D-3B56164BDBCF"); 
			var dienKhanh = new Guid("C54A2EC5-097B-46C3-8ECF-4246AD6DE48D"); 
			#endregion

			var huyens = new List<Huyen>
			{
				new Huyen(){ Id = tuyHoa ,TinhId = phuYen , Code="TH", Name="Thành phố Tuy Hoà"},
				new Huyen(){ Id = songCau ,TinhId = phuYen , Code="SC", Name="Thị xã Sông Cầu"},
				new Huyen(){ Id = tuyAn ,TinhId = phuYen , Code="TA", Name="Huyện Tuy An"},

				new Huyen(){ Id = nhaTrang ,TinhId = khanhHoa ,Code="NT", Name="Thành phố Nha Trang"},
				new Huyen(){ Id = ninhHoa ,TinhId = khanhHoa ,Code="NH", Name="Thị xã Ninh Hòa"},
				new Huyen(){ Id = camLam ,TinhId = khanhHoa ,Code="CL", Name="Huyện Cam Lâm"},
				new Huyen(){ Id = camRanh ,TinhId = khanhHoa ,Code="CR", Name="Thành phố Cam Ranh"},
				new Huyen(){ Id = dienKhanh ,TinhId = khanhHoa ,Code="DK", Name="Huyện Diên Khánh"},
				
			};

			context.Huyens.AddRange(huyens);
			await context.SaveChangesAsync();

			return huyens.Count;
		}

		public async static Task<int> SeedXa(ClickBuyDbContext context)
		{
			#region HuyenId
			var songCau = new Guid("223002AC-0E1B-4B93-A2B0-FAFBB6C98A05");
			var tuyAn = new Guid("F7734EB0-9C9E-411D-9794-89CDE95C69F2");
			var tuyHoa = new Guid("9DEAE3A4-80B4-40DD-84FA-EB2EDCB5503B");

			var camRanh = new Guid("0D837345-1437-4BCF-92CE-2AF554108800");
			var nhaTrang = new Guid("C473F5DB-F805-470F-90EF-65375B5FBB29");
			var ninhHoa = new Guid("DD264027-9B40-42BD-8765-82143AE7B424");
			var camLam = new Guid("11C04D5E-7990-4376-A43D-3B56164BDBCF");
			var dienKhanh = new Guid("C54A2EC5-097B-46C3-8ECF-4246AD6DE48D");
            #endregion

            #region Seed Xa
            var xas = new List<Xa>
			{
				new Xa(){ HuyenId = tuyHoa ,Code="555", Name="Phường 1"},
				new Xa(){ HuyenId = tuyHoa ,Code="555", Name="Phường 2"},
				new Xa(){ HuyenId = tuyHoa ,Code="555", Name="Phường 3"},
				new Xa(){ HuyenId = tuyHoa ,Code="555", Name="Phường 4"},
				new Xa(){ HuyenId = tuyHoa ,Code="555", Name="Phường 5"},
				new Xa(){ HuyenId = tuyHoa ,Code="555", Name="Phường 7"},
				new Xa(){ HuyenId = tuyHoa ,Code="555", Name="Phường 8"},
				new Xa(){ HuyenId = tuyHoa ,Code="555", Name="Phường 9"},
				new Xa(){ HuyenId = songCau, Code="557", Name="Phường Xuân Phú"},
				new Xa(){ HuyenId = songCau, Code="557", Name="Xã Xuân Lâm"},
				new Xa(){ HuyenId = songCau, Code="557", Name="Phường Xuân Thành"},
				new Xa(){ HuyenId = songCau, Code="557", Name="Xã Xuân Hải"},
				new Xa(){ HuyenId = songCau, Code="557", Name="Xã Xuân Lộc"},
				new Xa(){ HuyenId = songCau, Code="557", Name="Xã Xuân Bình"},
				new Xa(){ HuyenId = songCau, Code="557", Name="Xã Xuân Cảnh"},
				new Xa(){ HuyenId = songCau, Code="557", Name="Xã Xuân Thịnh"},
				new Xa(){ HuyenId = songCau, Code="557", Name="Xã Xuân Phương"},
				new Xa(){ HuyenId = songCau, Code="557", Name="Phường Xuân Yên"},
				new Xa(){ HuyenId = songCau, Code="557", Name="Xã Xuân Thọ 1"},
				new Xa(){ HuyenId = songCau, Code="557", Name="Phường Xuân Đài"},
				new Xa(){ HuyenId = songCau, Code="557", Name="Xã Xuân Thọ 2"},
				new Xa(){ HuyenId = tuyAn, Code="559", Name="Thị trấn Chí Thạnh"},
				new Xa(){ HuyenId = tuyAn, Code="559", Name="Xã An Dân"},
				new Xa(){ HuyenId = tuyAn, Code="559", Name="Xã An Ninh Tây"},
				new Xa(){ HuyenId = tuyAn, Code="559", Name="Xã An Ninh Đông"},
				new Xa(){ HuyenId = tuyAn, Code="559", Name="Xã An Thạch"},
				new Xa(){ HuyenId = tuyAn, Code="559", Name="Xã An Định"},
				new Xa(){ HuyenId = tuyAn, Code="559", Name="Xã An Nghiệp"},
				new Xa(){ HuyenId = tuyAn, Code="559", Name="Xã An Cư"},
				new Xa(){ HuyenId = tuyAn, Code="559", Name="Xã An Xuân"},
				new Xa(){ HuyenId = tuyAn, Code="559", Name="Xã An Lĩnh"},
				new Xa(){ HuyenId = tuyAn, Code="559", Name="Xã An Hòa Hải"},
				new Xa(){ HuyenId = tuyAn, Code="559", Name="Xã An Hiệp"},
				new Xa(){ HuyenId = tuyAn, Code="559", Name="Xã An Mỹ"},
				new Xa(){ HuyenId = tuyAn, Code="559", Name="Xã An Chấn"},
				new Xa(){ HuyenId = tuyAn, Code="559", Name="Xã An Thọ"},


				new Xa(){ HuyenId = camRanh ,Code="569", Name="Phường Cam Nghĩa"},
				new Xa(){ HuyenId = camRanh ,Code="569", Name="Phường Cam Phúc Bắc"},
				new Xa(){ HuyenId = camRanh ,Code="569", Name="Phường Cam Phúc Nam"},
				new Xa(){ HuyenId = camRanh ,Code="569", Name="Phường Cam Lộc"},
				new Xa(){ HuyenId = camRanh ,Code="569", Name="Phường Cam Phú"},
				new Xa(){ HuyenId = camRanh ,Code="569", Name="Phường Ba Ngòi"},
				new Xa(){ HuyenId = camRanh ,Code="569", Name="Phường Cam Thuận"},
				new Xa(){ HuyenId = camRanh ,Code="569", Name="Phường Cam Lợi"},
				new Xa(){ HuyenId = camRanh ,Code="569", Name="Phường Cam Linh"},
				new Xa(){ HuyenId = camRanh ,Code="569", Name="Xã Cam Thành Nam"},
				new Xa(){ HuyenId = camRanh ,Code="569", Name="Xã Cam Phước Đông"},
				new Xa(){ HuyenId = camRanh ,Code="569", Name="Xã Cam Thịnh Tây"},
				new Xa(){ HuyenId = camRanh ,Code="569", Name="Xã Cam Thịnh Đông"},
				new Xa(){ HuyenId = camRanh ,Code="569", Name="Xã Cam Lập"},
				new Xa(){ HuyenId = camRanh ,Code="569", Name="Xã Cam Bình"},
					
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Vĩnh Hòa"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Vĩnh Hải"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Vĩnh Phước"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Ngọc Hiệp"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Vĩnh Thọ"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Xương Huân"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Vạn Thắng"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Vạn Thạnh"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Phương Sài"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Phương Sơn"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Phước Hải"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Phước Tân"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Lộc Thọ"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Phước Tiến"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Tân Lập"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Phước Hòa"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Vĩnh Nguyên"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Phước Long"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Phường Vĩnh Trường"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Xã Vĩnh Lương"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Xã Vĩnh Phương"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Xã Vĩnh Ngọc"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Xã Vĩnh Thạnh"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Xã Vĩnh Trung"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Xã Vĩnh Hiệp"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Xã Vĩnh Thái"},
				new Xa(){ HuyenId = nhaTrang ,Code="568", Name="Xã Phước Đồng"},

				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Phường Ninh Hiệp"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Sơn"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Tây"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Thượng"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh An"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Phường Ninh Hải"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Thọ"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Trung"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Sim"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Xuân"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Thân"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Phường Ninh Diêm"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Đông"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Phường Ninh Thủy"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Phường Ninh Đa"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Phụng"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Bình"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Phước"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Phú"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Tân"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Quang"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Phường Ninh Giang"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Phường Ninh Hà"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Hưng"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Lộc"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Ích"},
				new Xa(){ HuyenId = ninhHoa ,Code="572", Name="Xã Ninh Vân"},
			};
            #endregion

            context.Xas.AddRange(xas);
			await context.SaveChangesAsync();

			return xas.Count;
		}


		private static void CreateUser(UserManager<User> userManager, RoleManager<Role> roleManager, ClickBuyDbContext context)
		{
            var phuYen = new Guid("4ABE246D-741D-40DB-A0BF-816A266FEF04");
            var tuyAn = new Guid("F7734EB0-9C9E-411D-9794-89CDE95C69F2");
			var anHiep = new Guid("996D7E66-AA13-4208-B3D6-85DA37593C39");
            var male = new Guid("0A42ABFA-FCAF-4BA0-8BEC-2331476888BF");


            var users = new List<User>
			{
				new User(){
                    ResetCode = "",
					Otp = "",
					FirstName = "Đặng Công",
					LastName = "Vinh",
					CreatedAt = DateTime.Now,
					IsActive = true,
					CreatedBy = "System",
					Phone = "0963562615",
					Address = "Khánh Hòa",
					ImageId = null,
					TinhId = phuYen,
					HuyenId = tuyAn,
					XaId = anHiep,
					GenderId = male,
					Email="dangcongvinh328@gmail.com",
					DateOfBirth = new DateTime(13,06,1999)},
				new User(){ UserName="mod", Email="mod@hmz.com", FirstName="Mod",LastName="Mod",DateOfBirth = new DateTime(01,01,1999)},
				new User(){ UserName="member", Email="user@hmz.com", FirstName="User",LastName="User",DateOfBirth = new DateTime(01,01,1999)}
			};
			// Save User to FILE
			var path = Directory.GetCurrentDirectory() + "\\Resoucres\\";
			var usersToSeed = System.Text.Json.JsonSerializer.Serialize(users);
			File.WriteAllText(path + "UserSeedData.json", usersToSeed);
		}
	}
}
