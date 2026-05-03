using UnityEngine;
using System.Collections;

public class HideSequence : MonoBehaviour
{
    [Header("ตั้งค่า Player")]
    public Transform player;
    [Tooltip("จุดที่จะให้ Player วาร์ปเข้าไปซ่อน")]
    public Transform hidePosition;
    
    [Tooltip("ใส่ก้อน Cube ล่องหน ที่เอาไว้ปิดทางออกตู้")]
    public GameObject closetDoorBlocker;

    [Header("ตั้งค่าเสียงฝีเท้าผี")]
    [Tooltip("Audio Source ที่ติดเสียงฝีเท้าผีไว้ (ไฟล์เสียงต้องเป็นก้าวเดียวสั้นๆ)")]
    public AudioSource ghostFootstepAudio;
    public Transform[] waypoints;
    
    [Header("ความหลอนของผี (Pacing)")]
    public float ghostSpeed = 0.5f;
    [Tooltip("จังหวะก้าวเท้า (วินาที) ค่าน้อย = ก้าวถี่, ค่ามาก = ก้าวช้าๆ หนักแน่น")]
    public float stepInterval = 1.5f;
    public float waitTimeAtWaypoint = 3f;
    
    [Header("เวลาซ่อนตัว")]
    public float hidingDuration = 90f;

    [Header("เหตุการณ์เมื่อรอดชีวิต (Safe Ending)")]
    public UnityEngine.Events.UnityEvent onSafeEnding;

    public void StartHidingSequence()
    {
        StartCoroutine(HidingRoutine());
    }

    IEnumerator HidingRoutine()
    {
        Debug.Log("[HideSequence] เริ่มการซ่อนตัว วาร์ปเข้าตู้!");

        // 1. วาร์ปอย่างปลอดภัย (ไม่เกิด Error แดง)
        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;
        
        player.position = hidePosition.position;
        player.rotation = hidePosition.rotation;
        
        if (cc != null) cc.enabled = true; 

        // 2. ปิดตู้ขังผู้เล่นด้วยกำแพงล่องหน (ผู้เล่นจะยังหันกล้องได้อิสระ)
        if (closetDoorBlocker != null) closetDoorBlocker.SetActive(true);

        // 3. เริ่มระบบผีเดินตรวจตรา
        float elapsed = 0f;
        int currentWp = 0;
        float stepTimer = stepInterval; 
        
        while (elapsed < hidingDuration)
        {
            if (waypoints.Length > 0 && ghostFootstepAudio != null)
            {
                Transform targetWp = waypoints[currentWp];
                
                ghostFootstepAudio.transform.position = Vector3.MoveTowards(
                    ghostFootstepAudio.transform.position, 
                    targetWp.position, 
                    ghostSpeed * Time.deltaTime
                );

                // --- ระบบสร้างเสียงทีละก้าวเท้า ---
                stepTimer += Time.deltaTime;
                if (stepTimer >= stepInterval)
                {
                    ghostFootstepAudio.Play(); 
                    stepTimer = 0f; 
                }

                if (Vector3.Distance(ghostFootstepAudio.transform.position, targetWp.position) < 0.1f)
                {
                    float waited = 0f;
                    while(waited < waitTimeAtWaypoint && elapsed < hidingDuration)
                    {
                        waited += Time.deltaTime;
                        elapsed += Time.deltaTime;
                        yield return null;
                    }
                    currentWp = (currentWp + 1) % waypoints.Length;
                    stepTimer = stepInterval; 
                }
            }
            
            elapsed += Time.deltaTime;
            yield return null; 
        }

        // 4. ครบเวลา (รอดชีวิต)
        if (ghostFootstepAudio != null) ghostFootstepAudio.Stop();
        
        // เปิดตู้ให้เดินออกได้ (ถ้าต้องการ)
        if (closetDoorBlocker != null) closetDoorBlocker.SetActive(false); 
        
        Debug.Log("[HideSequence] รอดชีวิต!");
        onSafeEnding.Invoke(); 
    }
}