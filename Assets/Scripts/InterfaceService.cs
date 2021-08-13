public interface HealthService 
{ 
    float GetCurrentHP(); 
}

public interface DamageService 
{ 
    void DamageTaken(float givenDamage); 
}
