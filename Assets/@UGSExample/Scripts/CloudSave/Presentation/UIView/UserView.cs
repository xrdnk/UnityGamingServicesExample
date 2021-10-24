using System;
using Shared.UIViewBase;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Denicode.UGSExample.CloudSave.Presentation.UIView
{
    public sealed class UserView : UIViewBase
    {
        [SerializeField] InputField _inputField;
        [SerializeField] Button _buttonRegister;
        [SerializeField] Button _buttonDelete;

        readonly Subject<string> _registerSubject = new Subject<string>();
        public IObservable<string> OnCreateTriggerAsObservable() => _registerSubject;

        readonly Subject<Unit> _deleteSubject = new Subject<Unit>();
        public IObservable<Unit> OnDeleteTriggerAsObservable() => _deleteSubject;

        protected override void Awake()
        {
            _buttonRegister.OnClickAsObservable()
                .Subscribe(_ => _registerSubject.OnNext(_inputField.text))
                .AddTo(this);

            _buttonDelete.OnClickAsObservable()
                .Subscribe(_ => _deleteSubject.OnNext(Unit.Default))
                .AddTo(this);

            _inputField.ObserveEveryValueChanged(x => x.text)
                .Subscribe(text => _buttonRegister.interactable = !string.IsNullOrEmpty(text))
                .AddTo(this);
        }
    }
}