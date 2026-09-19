using GorillaLocomotion;
using UnityEngine;

namespace Astras_PullMod_Hub.Core.MainSystems;

public static class PullMethods
{
    public enum PullMethod
    {
        PMV1,
        PMV2,
        Regular,
        Gravity,
        Surface,
        Burst,
        Inertia,
        Elastic,
        Directional,
        Curve,
        AirPull,
        GroundPull
    }

    public static PullMethod Method = PullMethod.PMV2;

    public static float PullPower = 0.025f;
    public static float UpHillPower = 0.020f;
    public static float Momentum = 0.001f;
    public static float MaxPull = 0.070f;
    public static float MaxVelocity = 15f;
    public static float ExtraPower = 1f;

    public static bool ClampVelocity = true;

    public static readonly string[] MethodNames =
    {
        "PMV1", "PMV2", "Regular",
        "Gravity", "Surface", "Burst",
        "Inertia", "Elastic", "Directional",
        "Curve", "Air Pull", "Ground Pull"
    };

    public static void Run()
    {
        switch (Method)
        {
            case PullMethod.PMV1: PMV1(); break;
            case PullMethod.PMV2: PMV2(); break;
            case PullMethod.Regular: Regular(); break;
            case PullMethod.Gravity: Gravity(); break;
            case PullMethod.Surface: Surface(); break;
            case PullMethod.Burst: Burst(); break;
            case PullMethod.Inertia: Inertia(); break;
            case PullMethod.Elastic: Elastic(); break;
            case PullMethod.Directional: Directional(); break;
            case PullMethod.Curve: Curve(); break;
            case PullMethod.AirPull: AirPull(); break;
            case PullMethod.GroundPull: GroundPull(); break;
        }
    }

    private static Vector3 Velocity()
    {
        Vector3 vel = GorillaTagger.Instance.rigidbody.linearVelocity;
        if (ClampVelocity)
            vel = Vector3.ClampMagnitude(vel, MaxVelocity);

        return vel;
    }

    public static void PMV1()
    {
        Vector3 vel = Velocity();
        GTPlayer.Instance.transform.position += new Vector3(vel.x * PullPower, vel.y * UpHillPower, vel.z * PullPower);
    }

    public static void PMV2()
    {
        Vector3 vel = Velocity();
        float speed = vel.magnitude;
        float power = Mathf.Clamp(PullPower + speed * Momentum, PullPower, PullPower * 8f);
        float boost = Mathf.Lerp(0.85f, 1.5f, Mathf.Clamp01(speed / Mathf.Max(MaxVelocity, 0.01f)));
        Vector3 pull = new Vector3(vel.x * power, vel.y * UpHillPower, vel.z * power) * boost;
        GTPlayer.Instance.transform.position += Vector3.ClampMagnitude(pull, MaxPull);
    }

    public static void Regular()
    {
        Vector3 vel = Velocity();
        float divisor = Mathf.Max(PullPower * 400f, 1f);
        Vector3 velocityPull = vel / divisor;
        Vector3 forwardPull = GTPlayer.Instance.bodyCollider.transform.forward * PullPower * ExtraPower;
        GTPlayer.Instance.transform.position += Vector3.ClampMagnitude(velocityPull + forwardPull, MaxPull);
    }

    public static void Gravity()
    {
        Rigidbody rb = GorillaTagger.Instance.rigidbody;
        Vector3 vel = Velocity();
        Vector3 pull = new(vel.x * PullPower, vel.y * UpHillPower, vel.z * PullPower);
        GTPlayer.Instance.transform.position += Vector3.ClampMagnitude(pull, MaxPull);
        rb.linearVelocity += Vector3.up * ExtraPower * 0.1f;
    }

    public static void Surface()
    {
        Vector3 vel = Velocity();
        if (!Physics.Raycast(GTPlayer.Instance.transform.position, Vector3.down, out RaycastHit hit, 5f))
        {
            PMV1();
            return;
        }
        Vector3 horizontal = Vector3.ProjectOnPlane(new Vector3(vel.x, 0f, vel.z), hit.normal) * PullPower;
        Vector3 pull = horizontal + Vector3.up * vel.y * UpHillPower;
        GTPlayer.Instance.transform.position += Vector3.ClampMagnitude(pull, MaxPull);
    }

    public static void Burst()
    {
        Rigidbody rb = GorillaTagger.Instance.rigidbody;
        Vector3 vel = Velocity();
        if (vel.sqrMagnitude < 0.001f)
            return;
        float burst = Mathf.Clamp(vel.magnitude * PullPower * ExtraPower, 0f, MaxPull * 2f);
        rb.linearVelocity += vel.normalized * burst;
    }

    public static void Inertia()
    {
        Rigidbody rb = GorillaTagger.Instance.rigidbody;
        Vector3 vel = Velocity();
        if (vel.sqrMagnitude < 0.001f)
            return;
        float boost = Mathf.Clamp(PullPower + vel.magnitude * Momentum, PullPower, MaxPull);
        rb.linearVelocity += vel.normalized * boost * ExtraPower;
    }

    public static void Elastic()
    {
        Vector3 vel = Velocity();
        float speed = vel.magnitude;
        float normalized = Mathf.Clamp01(speed / Mathf.Max(MaxVelocity, 0.01f));
        float elastic = Mathf.Sin(normalized * Mathf.PI * 0.5f);
        float power = Mathf.Lerp(PullPower * 0.25f, PullPower * ExtraPower, elastic);
        Vector3 pull = new(vel.x * power, vel.y * UpHillPower, vel.z * power);
        GTPlayer.Instance.transform.position += Vector3.ClampMagnitude(pull, MaxPull);
    }

    public static void Directional()
    {
        Vector3 vel = Velocity();
        Vector3 forward = GTPlayer.Instance.bodyCollider.transform.forward;
        float alignment = Mathf.Clamp01(Vector3.Dot(vel.normalized, forward));
        float multiplier = Mathf.Lerp(0.5f, ExtraPower, alignment);
        Vector3 pull = new(vel.x * PullPower * multiplier, vel.y * UpHillPower, vel.z * PullPower * multiplier);
        GTPlayer.Instance.transform.position += Vector3.ClampMagnitude(pull, MaxPull);
    }

    public static void Curve()
    {
        Vector3 vel = Velocity();
        float normalized = Mathf.Clamp01(vel.magnitude / Mathf.Max(MaxVelocity, 0.01f));
        float curve = normalized * normalized;
        float power = Mathf.Lerp(PullPower * 0.25f, PullPower * ExtraPower, curve);
        Vector3 pull = new(vel.x * power, vel.y * UpHillPower, vel.z * power);
        GTPlayer.Instance.transform.position += Vector3.ClampMagnitude(pull, MaxPull);
    }

    public static void AirPull()
    {
        bool touching = GTPlayer.Instance.IsHandTouching(true) || GTPlayer.Instance.IsHandTouching(false);
        Vector3 vel = Velocity();
        float multiplier = touching ? 0.35f : ExtraPower;
        Vector3 pull = new(vel.x * PullPower * multiplier, vel.y * UpHillPower * multiplier, vel.z * PullPower * multiplier);
        GTPlayer.Instance.transform.position += Vector3.ClampMagnitude(pull, MaxPull);
    }

    public static void GroundPull()
    {
        bool touching = GTPlayer.Instance.IsHandTouching(true) || GTPlayer.Instance.IsHandTouching(false);
        Vector3 vel = Velocity();
        float multiplier = touching ? ExtraPower : 0.25f;
        Vector3 pull = new(vel.x * PullPower * multiplier, vel.y * UpHillPower, vel.z * PullPower * multiplier);
        GTPlayer.Instance.transform.position += Vector3.ClampMagnitude(pull, MaxPull);
    }

    public static void Reset()
    {
        Method = PullMethod.PMV2;
        PullPower = 0.025f;
        UpHillPower = 0.020f;
        Momentum = 0.001f;
        MaxPull = 0.070f;
        MaxVelocity = 15f;
        ExtraPower = 1f;
        ClampVelocity = true;
    }
}