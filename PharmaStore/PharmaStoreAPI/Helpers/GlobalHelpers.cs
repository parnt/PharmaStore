namespace PharmaStoreAPI.Helpers
{
    using Core.Enums;
    using Core.Resources;
    using Core.ViewModels.Core;

    public class GlobalHelpers
    {
        public static OperationError ModelStateError()
        {
            return new OperationError
            {
                ErrorCode = (int) ErrorCodes.BadRequest,
                Message = ErrorResources.ModalStateError
            };
        }
    }
}
