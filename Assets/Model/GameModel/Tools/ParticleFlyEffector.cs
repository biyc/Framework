using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFlyEffector : MonoBehaviour
{
    [HideInInspector]
    public Transform f_pos;
    [HideInInspector]
    public Transform t_pos;

    public float time = 50;
    [HideInInspector]
    public ParticleSystem particleSys;
    private ParticleSystem.MainModule p_main;
    private ParticleSystem.Particle[] particles;
    private List<float> m_pStarter;
    private List<float> m_pLerp;
    private Action<ParticleSystem.Particle> p_EndAction;
    public void Awake()
    {
        this.particleSys = this.GetComponent<ParticleSystem>();

        p_main = particleSys.main;
        p_main.simulationSpeed = 2f;

        m_pStarter = new List<float>();
        m_pLerp = new List<float>();
    }

    Vector3 fromOfset;
    Vector3 toOfset;

    public ParticleFlyEffector Set(Transform _from,Transform _to, 
                                Vector3 _fromOfset, Vector3 _toOfset, 
                                int _MaxSize = 2, float WHrate = 1f, 
                                Action<ParticleSystem.Particle> _p_EndAction = null, 
                                float size = 1f)
    {
        if (!this.particleSys) return null;
        fromOfset = _fromOfset;
        toOfset = _toOfset;
        p_EndAction = _p_EndAction;

        f_pos = _from;
        f_pos.position = _from.position + fromOfset;
        t_pos = _to;
        t_pos.position = _to.position + toOfset;

        transform.position = f_pos.position;

        p_main.simulationSpace = ParticleSystemSimulationSpace.Custom;
        p_main.customSimulationSpace = this.t_pos;

        p_main.maxParticles = _MaxSize;

        if (WHrate != 1)
        {
            p_main.startSize3D = true;
            p_main.startSizeYMultiplier = WHrate;
        }
        else
            p_main.startSize3D = false;

        p_main.startSizeMultiplier = size;


        this.particles = new ParticleSystem.Particle[_MaxSize];


        for (int x = 0; x < _MaxSize; ++x)
        {
            m_pStarter.Add(UnityEngine.Random.Range(1f, 2f));
            m_pLerp.Add(0f);
        }
        return this;
    }

    private void Update()
    {
        //Debug.LogError("粒子判断");

        if (!f_pos || !t_pos)
        {
            return;
        }
        if (this.particleSys)
        {
            int count = this.particleSys.GetParticles(this.particles);
            for (int i = 0; i < count; ++i)
            {
                if (this.particles[i].remainingLifetime 
                    < p_main.startLifetime.constant - m_pStarter[i])
                {
                    m_pLerp[i] += Time.deltaTime;
                    this.particles[i].position = Vector3.Lerp(this.particles[i].position, Vector3.zero, m_pLerp[i] * 16 / time);
                }

                if (Vector3.Distance(this.particles[i].position, Vector3.zero) < 0.1f)
                {
                    this.particles[i].remainingLifetime = 0;
                    p_EndAction?.Invoke(this.particles[i]);
                }    
            }
            this.particleSys.SetParticles(this.particles, count);
            //Debug.LogError("粒子运行");
        }
    }

    private void OnEnable()
    {
        particleSys.Play();
    }

    private void OnDisable()
    {
        if(f_pos != null)
            //GOPool.Recicle(f_pos.gameObject);
        if(t_pos != null)
            //GOPool.Recicle(t_pos.gameObject);

        f_pos = null;
        t_pos = null;

        if (m_pLerp == null) return;
        m_pStarter.Clear();
        m_pLerp.Clear();
    }
}
