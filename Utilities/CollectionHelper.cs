namespace Utilities;

public static class CollectionHelper
{
    public static bool IsNullOrEmpty<T>(ICollection<T>? collection)
    {
        return collection == null || collection.Count == 0;
    }

    public static void Synchronize<T1, T2>(
        IList<T1> oldCollection,
        IList<T2> newCollection,
        Func<T1, T2, bool> equals,
        Func<T2, T1> create,
        Action<T1, T2> update)
    {
        var itemsFoundInNewCollection = new HashSet<T2>();

        for (int index = 0; index < oldCollection.Count; index++)
        {
            T1 oldItem = oldCollection[index];
            T2 newItem = default!;
            bool newItemFound = false;
            foreach (T2 newElement in newCollection)
            {
                if (equals(oldItem, newElement))
                {
                    newItem = newElement;
                    newItemFound = true;

                    break;
                }
            }

            if (newItemFound)
            {
                itemsFoundInNewCollection.Add(newItem);
                update(oldItem, newItem);
            }
            else
            {
                oldCollection.RemoveAt(index);
                index--;
            }
        }

        foreach (T2 newItem in newCollection.Except(itemsFoundInNewCollection))
        {
            T1 oldItem = create(newItem);
            update(oldItem, newItem);
            oldCollection.Add(oldItem);
        }
    }
}