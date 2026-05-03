using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class AutoTrigger : MonoBehaviour
{
    [Header("ตั้งค่าทริกเกอร์")]
    [Tooltip("ติ๊กถูกถ้าต้องการให้เริ่มนับเวลาทันทีที่เริ่มเกม (เช่น นับเวลาไฟดับ)")]
    public bool triggerOnStart = false;
    
    [Tooltip("หน่วงเวลากี่วินาทีก่อน Event จะทำงาน")]
    public float delayTime = 0f;
    
    [Tooltip("Event ที่ต้องการให้เกิดขึ้น (ลาก Object มาใส่แล้วเลือกฟังก์ชันได้เลย)")]
    public UnityEvent onTrigger;

    private bool hasTriggered = false;

    void Start()
    {
        // ถ้าให้ทำงานตั้งแต่เริ่มฉาก ก็เริ่มนับเวลาเลย
        if (triggerOnStart)
        {
            StartCoroutine(TriggerRoutine());
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // ถ้าเป็นแบบเดินมาเหยียบ (ไม่นับเวลาแต่แรก) และคนที่เหยียบมี Tag "Player"
        if (!triggerOnStart && !hasTriggered && other.CompareTag("Player"))
        {
            StartCoroutine(TriggerRoutine());
        }
    }

    IEnumerator TriggerRoutine()
    {
        hasTriggered = true; // ล็อกไว้ไม่ให้ทำงานซ้ำ
        
        if (delayTime > 0)
        {
            yield return new WaitForSeconds(delayTime);
        }
        
        // สั่งให้ Event ทำงาน
        onTrigger.Invoke();
        Debug.Log($"[AutoTrigger] ทำงานแล้วบน Object: {gameObject.name}");
    }
}