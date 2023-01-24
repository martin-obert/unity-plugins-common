using System.Collections.Generic;
using System.Linq;
using Obert.Common.Runtime.Extensions;
using Obert.Common.Runtime.Repositories;
using Obert.Common.Runtime.Repositories.Components;
using Obert.Common.Runtime.Repositories.Events;
using UnityEngine;

public class PlayerAccountList : MonoBehaviour
{
    [SerializeField] private PlayerAccountListItem itemPrefab;
    [SerializeField] private Transform container;

    public void OnItemCreated(ItemCreatedEvent<PlayerAccount> @event)
    {
        Instantiate(itemPrefab, container).Bind(@event.Item);
    }

    public void OnItemDeletedBulk(ItemDeletedBulkEvent<PlayerAccount> @event)
    {
        var accountItems = GetComponentsInChildren<PlayerAccountListItem>()
            .Where(x => @event.Items.Contains(x.PlayerAccount));

        foreach (var accountListItem in accountItems)
        {
            Destroy(accountListItem.gameObject);
        }
    }


    public void OnItemDeleted(ItemDeletedEvent<PlayerAccount> @event)
    {
        var accountItems = GetComponentsInChildren<PlayerAccountListItem>()
            .Where(x => @event.Item == x.PlayerAccount);

        foreach (var accountListItem in accountItems)
        {
            Destroy(accountListItem.gameObject);
        }
    }
}