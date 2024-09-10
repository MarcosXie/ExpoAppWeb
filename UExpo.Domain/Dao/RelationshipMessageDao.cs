using UExpo.Domain.Dao.Shared;

namespace UExpo.Domain.Dao;

public class RelationshipMessageDao : BaseMessageDao
{
    public RelationshipDao Chat { get; set; } = null!;
}
