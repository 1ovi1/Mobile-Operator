using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MobileOperator.models;

class RateListModel : INotifyPropertyChanged
{
    private List<RateModel> allCorporateRates;
    private List<RateModel> allNotCorporateRates;
    private List<RateModel> allRates;
    private readonly Infrastructure.MobileOperator _context;

    public RateListModel(Infrastructure.MobileOperator context)
    {
        _context = context;
        LoadRates();
    }

    private void LoadRates()
    {
        var rates = _context.Rate.ToList();

        allCorporateRates = rates
            .Where(i => i.Corporate == true)
            .Select(i => new RateModel(i, _context))
            .ToList();

        allNotCorporateRates = rates
            .Where(i => i.Corporate == false)
            .Select(i => new RateModel(i, _context))
            .ToList();

        allRates = rates
            .Select(i => new RateModel(i, _context))
            .ToList();
    }

    public List<RateModel> AllCorporateRates => allCorporateRates;
    public List<RateModel> AllNotCorporateRates => allNotCorporateRates;
    public List<RateModel> AllRates => allRates;

    public event PropertyChangedEventHandler PropertyChanged;
    public void OnPropertyChanged([CallerMemberName] string prop = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}