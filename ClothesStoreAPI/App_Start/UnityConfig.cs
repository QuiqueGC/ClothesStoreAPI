using ClothesStoreAPI.Models;
using ClothesStoreAPI.Repository.DB;
using ClothesStoreAPI.Repository.DB.Clothes;
using ClothesStoreAPI.Repository.DB.ClothesDeleted;
using ClothesStoreAPI.Repository.DB.Colors;
using ClothesStoreAPI.Repository.DB.Sizes;
using ClothesStoreAPI.Repository.DBManager;
using System;
using System.Web.Http;
using Unity;
using Unity.AspNet.Mvc;
using Unity.AspNet.WebApi;

namespace ClothesStoreAPI
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static readonly Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();
              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
            container.RegisterType<IClothesStoreEntities, ClothesStoreEntities>();
            container.RegisterType<IColorsRepository, ColorsRepository>();
            container.RegisterType<ISizesRepository, SizesRepository>();
            container.RegisterType<IClothesRepository, ClothesRepository>();
            container.RegisterType<IClothesDeletedRepository, ClothesDeletedRepository>();
        }   
            
        internal static void RegisterComponents()
        {
            var container = new UnityContainer();

            // Register all your components with the container here
            // e.g. container.RegisterType<ITestService, TestService>();

            RegisterTypes(container);

            // Set the dependency resolver for MVC
            System.Web.Mvc.DependencyResolver.SetResolver(
                new Unity.AspNet.Mvc.UnityDependencyResolver(container)
                );

            // Set the dependency resolver for Web API
            GlobalConfiguration.Configuration.DependencyResolver =
                new Unity.AspNet.WebApi.UnityDependencyResolver(container);

        }
    }
}