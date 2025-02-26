using UnityEngine;

namespace Core.Utilities
{
    public class ParticlesWithAttractor : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private ParticleSystemForceField _particleSystemForceField;

        public void SetAttractorPos(Vector3 pos)
        {
            _particleSystemForceField.transform.position = pos;
            ConfigureAttractionDistance();
        }

        public void SetParticlesPos(Vector3 pos)
        {
            _particleSystem.transform.position = pos;
            var dir = _particleSystemForceField.transform.position - pos;
            dir.Normalize();
            _particleSystem.transform.rotation = Quaternion.LookRotation(dir);
            ConfigureAttractionDistance();
        }

        private void ConfigureAttractionDistance()
        {
            var distance = Vector3.Distance(_particleSystem.transform.position, _particleSystemForceField.transform.position);
            _particleSystemForceField.endRange = distance * 1.2f;
        }

        public void Emit(int particlesAmount)
        {
            _particleSystem.Emit(particlesAmount);
        }
    }
}