using PriceAnalysis.BLL.Models;
using Radzen;

namespace PriceAnalysis.BLL.Services;

public interface ISynchSpecificationAndKPService
{
    List<SynchSpecificationAndKPResultDto> GetSynchSpecificationAndKP(int sectionId);
    NotificationMessage CreateItemSynchSpecificationAndKPPrep(int sectionId);
    List<SynchSpecificationAndKPPrepDto> GetListSynchSpecificationAndKPPrep(Guid projectId);
    NotificationMessage DeleteItemSynchSpecificationAndKPPrep(int sectionId);
    NotificationMessage Synchronization(int sectionId);
    NotificationMessage ReSynchronization(int sectionId);
}
