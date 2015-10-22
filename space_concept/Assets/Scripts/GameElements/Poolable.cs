using UnityEngine;
using System.Collections;

/**
 * Interface for Objects that can be handled with the pool allocator.
 */
public interface Poolable {
    void Reset();	            // Rests the object to the initial state. Used when an Object taken out of the pool.
}
