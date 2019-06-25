using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleScript : MonoBehaviour
{
    HammerControl hammerControl;
    //　パーティクルシステム
    private ParticleSystem ps;
    //　ScaleUp用の経過時間
    private float elapsedScaleUpTime = 0f;
    //　Scaleを大きくする間隔時間
    [SerializeField]
    private float scaleUpTime = 0.03f;
    //　ScaleUpする割合
    [SerializeField]
    private float scaleUpParam = 0.1f;
    //　パーティクル削除用の経過時間
    private float elapsedDeleteTime = 0f;
    //　パーティクルを削除するまでの時間
    [SerializeField]
    private float deleteTime = 5f;

    //　元のパーティクルの透明度
    private float alphaValue = 1f;
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedScaleUpTime += Time.deltaTime;
        elapsedDeleteTime += Time.deltaTime;

        if (elapsedDeleteTime >= deleteTime)
        {
            Destroy(gameObject);
        }

        if (elapsedScaleUpTime > scaleUpTime)
        {
            transform.localScale += new Vector3(scaleUpParam, scaleUpParam, scaleUpParam);
            elapsedScaleUpTime = 0f;
        }
    }
    public void OnParticleTrigger()
    {

        if (ps != null)
        {

            //　パーティクルを段々と透けさせる処理
            List<ParticleSystem.Particle> outside = new List<ParticleSystem.Particle>();

            int numOutside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Outside, outside);

            alphaValue -= (1f / deleteTime) * Time.deltaTime;

            alphaValue = (alphaValue <= 0f) ? 0f : alphaValue;

            for (int i = 0; i < numOutside; i++)
            {
                ParticleSystem.Particle p = outside[i];
                p.startColor = new Color(1f, 1f, 1f, alphaValue);
                outside[i] = p;
            }

            ps.SetTriggerParticles(ParticleSystemTriggerEventType.Outside, outside);

        }

    }
}