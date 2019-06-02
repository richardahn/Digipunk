using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Physics2DUtility
{
    public static Collider2D FindClosestTarget(GameObject user, float radius, LayerMask layerMask)
    {
        // Find nearby targets
        Vector2 userPosition = user.transform.position;
        List<Collider2D> targets = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(layerMask);
        Physics2D.OverlapCircle(userPosition, radius, filter, targets);
        Debug.Log("Found # targets = " + targets.Count);
        // Sort by distance
        targets.Sort((Collider2D c1, Collider2D c2) => {
            return Vector2.Distance(userPosition, c1.transform.position).CompareTo(Vector2.Distance(user.transform.position, c2.transform.position));
        });
        // Linecast starting from smallest
        foreach (Collider2D target in targets)
        {
            if (ReferenceEquals(target.gameObject, user))
                continue;

            RaycastHit2D hit = Physics2D.Linecast(userPosition, target.transform.position, layerMask.value);
            Debug.Log("Testing linecast on target = " + hit.collider.name);
            if (hit.collider == target)
            {
                Debug.Log("Found target = " + target.name);
                return target;
            }
        }
        Debug.Log("Could not find target");
        return null;
    }
}
