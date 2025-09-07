using Infrastructure.Command.Heterochromic;
using Infrastructure.Command.TransactionSupportive;

namespace Comp_v3.Front.DataGrid.CondDesign.Transactions;

public interface IAddingNewItemTransaction : ITransaction<IDeferredCommand>
{
    
}

public class AddingNewItemTransaction : TransactionDeferredSupportive {}