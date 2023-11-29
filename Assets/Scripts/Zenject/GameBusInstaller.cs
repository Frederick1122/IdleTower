using Configs;
using UnityEngine;
using Zenject;

public class GameBusInstaller : MonoInstaller
{
    [SerializeField] private Tower _tower;
    [SerializeField] private UpgradesConfig _upgradesConfig;
    
    public override void InstallBindings()
    {
        Container.Bind<Tower>().FromInstance(_tower).AsSingle();
        Container.Bind<UpgradesConfig>().FromInstance(_upgradesConfig).AsSingle();
        
        Container.QueueForInject(_upgradesConfig);
    }
}