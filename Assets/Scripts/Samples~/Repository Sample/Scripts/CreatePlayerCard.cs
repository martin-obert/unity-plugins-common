using Obert.Common.Runtime.Providers;
using Obert.Common.Runtime.Repositories;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class CreatePlayerCard : MonoBehaviour
{
    [SerializeField] private TMP_InputField username;

    private IRepository<PlayerAccount> _repository;

    private void Start()
    {
        _repository = RepositoryProvider.Instance.ProvideRepositoryFor<PlayerAccount>();
        Assert.IsNotNull(_repository);
    }

    public void Create()
    {
        var usernameText = username.text;

        if (string.IsNullOrWhiteSpace(usernameText)) return;

        _repository.AddSingle(new PlayerAccount { Username = usernameText });
        _repository.Save();
    }
}