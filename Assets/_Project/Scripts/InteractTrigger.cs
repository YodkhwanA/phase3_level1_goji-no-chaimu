using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using TMPro;

public class InteractTrigger : MonoBehaviour
{
    [Header("ตั้งค่า UI และข้อความ")]
    public string interactText = "Press E";
    [Tooltip("ลาก UI TextMeshPro มาใส่ตรงนี้")]
    public TextMeshProUGUI uiText; 
    
    [Header("ตั้งค่าเหตุการณ์")]
    public UnityEvent onInteract;
    
    [Tooltip("ติ๊กถูกถ้าต้องการให้กดได้แค่ครั้งเดียว")]
    public bool disableAfterUse = true;

    private bool isPlayerInRange = false;
    private bool isUsed = false;

    void Update()
    {
        // ตรวจสอบว่าผู้เล่นอยู่ในระยะ และ Object นี้ยังไม่ถูกใช้งาน
        if (isPlayerInRange && !isUsed)
        {
            // ตรวจจับปุ่ม E จาก New Input System (ใช้วิธีเรียกตรงๆ เพื่อความรวดเร็วใน Greybox)
            if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (disableAfterUse) isUsed = true;
                
                // ซ่อน UI
                if (uiText != null) uiText.gameObject.SetActive(false);
                
                // เรียก Event
                onInteract.Invoke();
                Debug.Log($"[InteractTrigger] Press E: {gameObject.name}");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!isUsed && other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            if (uiText != null)
            {
                uiText.text = interactText;
                uiText.gameObject.SetActive(true); // โชว์ UI
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            if (uiText != null) uiText.gameObject.SetActive(false); // ซ่อน UI เมื่อเดินออก
        }
    }
}