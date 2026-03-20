using Infrastructure.Command.Heterochromic;

namespace WPF.Templates.TableWindow.v1.Operations.Transactions;

public class Transactions { }

public class TransactionAddItem : TransactionDeferredSupportive { }
public class TrSelectingCell : TransactionDeferredSupportive { }
public class TrEditCell : TransactionDeferredSupportive { }
public class TrDeleteCell : TransactionDeferredSupportive { }