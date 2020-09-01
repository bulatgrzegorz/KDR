using System;
using System.Transactions;

namespace KDR.Transactions
{
    public class GenericTransaction : IEnlistmentNotification
    {
        public static void EnlistTransaction(Action prepare, Action commit, Action rollback, Action inDoubt)
        {
            var st = new GenericTransaction(prepare, commit, rollback, inDoubt);
            Transaction.Current.EnlistVolatile(st, EnlistmentOptions.None);
        }

        private readonly Action CommitAction;
        private readonly Action PrepareAction;
        private readonly Action RollbackAction;
        private readonly Action InDoubtAction;

        private GenericTransaction(Action prepare, Action commit, Action rollback, Action inDoubt)
        {
            this.CommitAction = commit ?? (Action)(() => { });
            this.PrepareAction = prepare ?? (Action)(() => { });
            this.RollbackAction = rollback ?? (Action)(() => { });
            this.InDoubtAction = inDoubt ?? (Action)(() => { });
        }

        public void Prepare(PreparingEnlistment preparingEnlistment)
        {
            try
            {
                //TODO: To wykonuje się po commicie i możemy wykonać akcje przygotowujące do commita. Po tym kroku commit powinien wykonać się zawsze!
                PrepareAction();
                preparingEnlistment.Prepared();
            }
            catch
            {
                preparingEnlistment.ForceRollback();
            }

        }

        public void Commit(Enlistment enlistment)
        {
            //TODO: Ostateczna akcja mająca zatwierdzić zmiany. Powinna wykoanać się zawsze bez błędu.
            CommitAction();
            enlistment.Done();
        }

        public void Rollback(Enlistment enlistment)
        {
            RollbackAction();
            enlistment.Done();
        }

        public void InDoubt(Enlistment enlistment)
        {
            InDoubtAction();
            enlistment.Done();
        }
    }
}