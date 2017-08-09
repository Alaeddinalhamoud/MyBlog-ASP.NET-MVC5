[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(MyBlog.UI.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(MyBlog.UI.App_Start.NinjectWebCommon), "Stop")]

namespace MyBlog.UI.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    using System.Web.Security;
    using Service;
    using Repo;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IPostRepository>().To<EFPostRepository>();
            kernel.Bind<IUserRepository>().To<EFUserRepository>();
            kernel.Bind<IAuthentication>().To<FormsAuthenticationProvider>();
            kernel.Bind<IImageRepository>().To<EFImageRespository>();
            kernel.Bind<ICategoryRepository>().To<EFCategoryRepository>();
            kernel.Bind<ICommentRepository>().To<EFCommentRepository>();
            kernel.Bind<ISettingRepository>().To<EFSettingRepository>();
            kernel.Bind<IWidgetRepository>().To<EFWidgetRepository>();
            kernel.Bind<IPageRepository>().To<EFPageRepository>();
            kernel.Bind<MembershipProvider>().To<EFMembershipProvider>();
            kernel.Bind<RoleProvider>().To<EFRoleProvider>();
            kernel.Bind<IRoleRepository>().To<EFRoleRepository>();
            kernel.Bind<IDEncryptionRepository>().To<EFEncryptionRepository>();
            kernel.Bind<IEmailSettingRepository>().To<EFEmailSettingRepository>();
        }        
    }
}
