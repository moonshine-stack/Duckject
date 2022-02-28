using Duckject.Core.Attributes;
using UnityEngine;

namespace Duckject.Examples._2_MonoInject.Scripts
{
    public class Rotator : MonoBehaviour
    {
        private Transform _object;

        private float _speed;

        [Quack]
        private void Construct(Transform target, float speed) => (_object, _speed) = (target, speed);

        private void Update() => _object.Rotate(Vector3.one, Time.deltaTime * _speed);
    }
}