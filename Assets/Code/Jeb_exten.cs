using UnityEngine;
//function to be called by other script(plz talk to be before changing)
public static class Jeb_exten
{
    //raycast looks for things tagged "default" so it does not register player in raycast and cause infinite jump
    private static LayerMask layerMask = LayerMask.GetMask("Default");
    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction)
    {
        //creates a new raycast
        if(rigidbody.isKinematic)
        {
            return false;
        }
        //sets sizie for raycast
        float radius = 0.25f;
        //how far down the raycast goes, set so that it just barly peeks down from player feet
        //can test this by moving players current capsule collider to these settings
        float distance = 0.375f;

        //sets the type of collider with previous set rules
         RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction, distance, layerMask);
         return hit.collider != null && hit.rigidbody != rigidbody;

    }

    public static bool DotTest(this Transform transform, Transform other, Vector2 testDirection)
    {
        Vector2 direction = other.position - transform.position;
        return Vector2.Dot(direction, testDirection) > 0f;
    }
}
