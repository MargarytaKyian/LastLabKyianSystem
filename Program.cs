using System;
using System.Windows.Forms;

namespace EasyTalk
{
    internal static class Program
    {
        public static AdminsDBAdminsForm mainAddAdminsForm;
        public static AddAdminsForm addAdminsForm;
        public static UpdateAdminsForm updateAdminsForm;

        public static ClientsDBAdminsForm mainAddClientsForm;
        public static AddClientsForm addClientsForm;
        public static UpdateClientsForm updateClientsForm;

        public static CellOpersDBAdminsForm mainAddCellOpersForm;
        public static AddCellOpersForm addCellOpersForm;
        public static UpdateCellOpersForm updateCellOpersForm;

        public static ConnectionsDBAdminsForm mainAddConnectsForm;
        public static AddConnectsForm addConnectsForm;
        public static UpdateConnectsForm updateConnectsForm;

        public static TariffPlansDBAdminsForm mainAddTariffsForm;
        public static AddTariffsForm addTariffsForm;
        public static UpdateTariffsForm updateTariffsForm;

        public static TransactionsDBAdminsForm mainAddTransactsForm;
        public static AddTransactsForm addTransactsForm;
        public static UpdateTransactsForm updateTransactsForm;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            addAdminsForm = new AddAdminsForm();
            updateAdminsForm = new UpdateAdminsForm();
            mainAddAdminsForm = new AdminsDBAdminsForm(addAdminsForm, updateAdminsForm);

            addClientsForm = new AddClientsForm();
            updateClientsForm = new UpdateClientsForm();
            mainAddClientsForm = new ClientsDBAdminsForm(addClientsForm, updateClientsForm);

            addCellOpersForm = new AddCellOpersForm();
            updateCellOpersForm = new UpdateCellOpersForm();
            mainAddCellOpersForm = new CellOpersDBAdminsForm(addCellOpersForm, updateCellOpersForm);

            addConnectsForm = new AddConnectsForm();
            updateConnectsForm = new UpdateConnectsForm();
            mainAddConnectsForm = new ConnectionsDBAdminsForm(addConnectsForm, updateConnectsForm);

            addTariffsForm = new AddTariffsForm();
            updateTariffsForm = new UpdateTariffsForm();
            mainAddTariffsForm = new TariffPlansDBAdminsForm(addTariffsForm, updateTariffsForm);

            addTransactsForm = new AddTransactsForm();
            updateTransactsForm = new UpdateTransactsForm();
            mainAddTransactsForm = new TransactionsDBAdminsForm(addTransactsForm, updateTransactsForm);

            Application.Run(new LoginForm());
        }
    }
}

