using System.Collections.Generic;
using UnityEngine;

public class RamRingsEffect : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private SpriteRenderer ramPrefab;   // your "राम" sprite

    [Header("Rings")]
    [SerializeField] private int ringCount = 6;
    [SerializeField] private int itemsPerRing = 14;
    [SerializeField] private float minRadius = 0.3f;
    [SerializeField] private float maxRadius = 3f;
    [SerializeField] private float itemScale = 0.12f;    // text size relative to the ring

    [Header("Motion")]
    [SerializeField] private float expandSpeed = 0.15f;  // rings per second moving outward
    [SerializeField] private float rotateSpeed = 12f;    // degrees per second, alternates direction

    [Header("Look")]
    [SerializeField] private Color innerColor = new Color(1f, 0.9f, 0.4f, 1f);  // bright gold near center
    [SerializeField] private Color outerColor = new Color(1f, 0.35f, 0.05f, 1f); // deep orange at edge
    [SerializeField]
    private AnimationCurve fade = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.15f, 1f),
        new Keyframe(0.7f, 1f),
        new Keyframe(1f, 0f));
    [SerializeField] private int sortingOrder = 0;

    private class Ring
    {
        public Transform root;
        public List<SpriteRenderer> items = new List<SpriteRenderer>();
        public float direction;
    }

    private readonly List<Ring> rings = new List<Ring>();

    private void Start()
    {
        for (int r = 0; r < ringCount; r++)
            rings.Add(BuildRing(r));
    }

    private Ring BuildRing(int index)
    {
        var ring = new Ring
        {
            root = new GameObject("Ring_" + index).transform,
            direction = index % 2 == 0 ? 1f : -1f
        };

        ring.root.SetParent(transform, false);

        // Offset every other ring by half a step so words don't line up in columns
        float angleOffset = (index % 2) * (Mathf.PI / itemsPerRing);

        for (int i = 0; i < itemsPerRing; i++)
        {
            float a = angleOffset + (i / (float)itemsPerRing) * Mathf.PI * 2f;

            SpriteRenderer sr = Instantiate(ramPrefab, ring.root);
            sr.transform.localPosition = new Vector3(Mathf.Sin(a), Mathf.Cos(a), 0f);

            // Top of the text points away from the center
            sr.transform.localRotation = Quaternion.Euler(0f, 0f, -a * Mathf.Rad2Deg);
            sr.transform.localScale = Vector3.one * itemScale;
            sr.sortingOrder = sortingOrder;

            ring.items.Add(sr);
        }

        return ring;
    }

    private void Update()
    {
        float t = Time.time;

        for (int i = 0; i < rings.Count; i++)
        {
            Ring ring = rings[i];

            // Each ring is offset in phase, so they flow out one after another
            float phase = Mathf.Repeat(t * expandSpeed + i / (float)ringCount, 1f);

            float radius = Mathf.Lerp(minRadius, maxRadius, phase);
            ring.root.localScale = Vector3.one * radius;
            ring.root.localRotation = Quaternion.Euler(0f, 0f, ring.direction * rotateSpeed * t);

            Color c = Color.Lerp(innerColor, outerColor, phase);
            c.a = fade.Evaluate(phase);

            for (int k = 0; k < ring.items.Count; k++)
                ring.items[k].color = c;
        }
    }
}