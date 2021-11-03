using System;
using System.Collections.Generic;
using System.Linq;
using Denicode.UGSExample.Shared.UIViewBase;
using UniRx;
using UniRx.Triggers;
using Unity.Services.Relay.Models;
using UnityEngine;
using UnityEngine.UI;

namespace Denicode.UGSExample.RelayService.Presentation.View
{
    public sealed class RelayView : UIViewBase
    {
        [SerializeField] Button _buttonGetRegions;
        [SerializeField] Button _buttonCreateRelay;
        [SerializeField] Button _buttonGetJoinCode;
        [SerializeField] Button _buttonJoinRelay;

        [SerializeField] Dropdown _dropDownRegions;
        [SerializeField] Text _textHostAllocationId;
        [SerializeField] Text _textJoinCode;
        [SerializeField] Text _textPlayerAllocationId;

        readonly Subject<Unit> _getRegionsSubject = new();
        public IObservable<Unit> OnGetRegionsAsObservable() => _getRegionsSubject;

        readonly Subject<Region> _createRelaySubject = new();
        public IObservable<Region> OnCreateRelayAsObservable() => _createRelaySubject;

        readonly Subject<Unit> _getJoinCodeSubject = new();
        public IObservable<Unit> OnGetJoinCodeAsObservable() => _getJoinCodeSubject;

        readonly Subject<Unit> _joinRelaySubject = new();
        public IObservable<Unit> OnJoinRelayAsObservable() => _joinRelaySubject;

        List<Region> _regions = new();

        protected override void Awake()
        {
            _buttonGetRegions.OnClickAsObservable()
                .Subscribe(_ => _getRegionsSubject.OnNext(Unit.Default))
                .AddTo(this);

            _buttonCreateRelay.OnClickAsObservable()
                .Subscribe(_ => _createRelaySubject.OnNext(_regions[_dropDownRegions.value]))
                .AddTo(this);

            _buttonGetJoinCode.OnClickAsObservable()
                .Subscribe(_ => _getJoinCodeSubject.OnNext(Unit.Default))
                .AddTo(this);

            _buttonJoinRelay.OnClickAsObservable()
                .Subscribe(_ => _joinRelaySubject.OnNext(Unit.Default))
                .AddTo(this);

            this.UpdateAsObservable()
                .Subscribe(_ => _dropDownRegions.interactable = _regions.Count > 0)
                .AddTo(this);
        }

        public void SetRegions(List<Region> regions)
        {
            _regions = regions;
            _dropDownRegions.ClearOptions();
            _dropDownRegions.AddOptions(_regions.Select(x => x.Description).ToList());
            _dropDownRegions.RefreshShownValue();
        }

        public void DisplayHostAllocationId(Guid guid) => _textHostAllocationId.text = $"{guid}";

        public void DisplayJoinCode(string joinCode) => _textJoinCode.text = $"{joinCode}";

        public void DisplayPlayerAllocationId(Guid guid) => _textPlayerAllocationId.text = $"{guid}";

        protected override void OnDestroy()
        {
            _getRegionsSubject.Dispose();
            _createRelaySubject.Dispose();
            _getJoinCodeSubject.Dispose();
            _joinRelaySubject.Dispose();
        }
    }
}