using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace BienesRaicesAPI.Tools
{
    public class SwaggerGroupByVersion : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var namespaceController = controller.ControllerType.Namespace!;
            var versionAPI = namespaceController.Split(".")[^1].ToLower();
            controller.ApiExplorer.GroupName = versionAPI;
        }
    }
}
