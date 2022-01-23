namespace eSDSCom.Editor.Shared.Models;

//source of substance data: https://echa.europa.eu/information-on-chemicals/registered-substances
//TODO: discuss licensed use?
// downloaded on 12/28/2021
public class Substance
{
    public Substance(){ }

    public Guid SubstanceId { get; set; }
    public string ID { get; set; }  

    public string Name { get; set; }

    public string ECNumber { get; set; }

    public string CASNumber { get; set; }

    public string RegistrationStatus { get; set; }

    public string RegistrationType { get; set; }

    public string SubmissionType { get; set; }

    public string TonnageBand { get; set; }

    public string TonnageBandMin { get; set; }

    public string TonnageBandMax { get; set; }

    public string LastUpdated { get; set; }

    public string ViewLink { get; set; }

    public string SubstanceInformationLink { get; set; }

}
