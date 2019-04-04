namespace PharmaStoreAPI.Helpers
{
    using Core.Enums;
    using Core.Resources;
    using Core.ViewModels.Core;

    public class GlobalHelpers
    {
        public static OperationError ModelStateError() =>
            new OperationError((int) ErrorCodes.BadRequest, ErrorResources.ModalStateError);
    }
}
