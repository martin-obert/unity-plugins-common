using Obert.Common.Runtime.Providers;
using TMPro;
using UnityEngine;

public class PlayerAccountListItem : MonoBehaviour
{
    [SerializeField] private TMP_Text label;
    public PlayerAccount PlayerAccount { get; private set; }

    public void Delete()
    {
        RepositoryProvider.Instance.ProvideRepositoryFor<PlayerAccount>().DeleteSingle(PlayerAccount);
    }

    public void Bind(PlayerAccount account)
    {
        PlayerAccount = account;
        label.text = account.Username;
    }
}