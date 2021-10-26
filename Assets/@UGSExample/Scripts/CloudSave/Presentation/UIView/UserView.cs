using System;
using System.Collections.Generic;
using CloudSave.Application.Enumerate;
using Denicode.UGSExample.CloudSave.Domain.Entity;
using Shared.UIViewBase;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Denicode.UGSExample.CloudSave.Presentation.UIView
{
    public sealed class UserView : UIViewBase
    {
        [SerializeField] InputField _inputFieldName;
        [SerializeField] InputField _inputFieldAge;

        [SerializeField] Dropdown _dropdownDataType;
        [SerializeField] Button _buttonSaveData;
        [SerializeField] Button _buttonReadData;
        [SerializeField] Button _buttonDeleteData;

        readonly Subject<DataType> _readSubject = new Subject<DataType>();
        public IObservable<DataType> OnReadTriggerAsObservable() => _readSubject;

        // NOTE: 面倒なのでここでは object 型にしている
        readonly Subject<(DataType, object)> _createSubject = new Subject<(DataType, object)>();
        public IObservable<(DataType, object)> OnCreateTriggerAsObservable() => _createSubject;

        readonly Subject<DataType> _deleteSubject = new Subject<DataType>();
        public IObservable<DataType> OnDeleteTriggerAsObservable() => _deleteSubject;

        DataType DropDownDataTypeValue => (DataType)Enum.Parse(typeof(DataType), $"{_dropdownDataType.value}");

        protected override void Awake()
        {
            InitDropDown();

            _buttonSaveData.OnClickAsObservable()
                .Subscribe(_ => NotifySaveData(_createSubject, DropDownDataTypeValue))
                .AddTo(this);

            _buttonReadData.OnClickAsObservable()
                .Subscribe(_ => _readSubject.OnNext(DropDownDataTypeValue))
                .AddTo(this);

            _buttonDeleteData.OnClickAsObservable()
                .Subscribe(_ => _deleteSubject.OnNext(DropDownDataTypeValue))
                .AddTo(this);
        }

        void InitDropDown()
        {
            var enumNames = Enum.GetNames(typeof(DataType));
            var names = new List<string>(enumNames);
            _dropdownDataType.AddOptions(names);
        }

        void NotifySaveData(IObserver<(DataType, object)> subject, DataType dataType)
        {
            switch (dataType)
            {
                case DataType.Name:
                    subject.OnNext((DropDownDataTypeValue, _inputFieldName.text));
                    break;
                case DataType.Age:
                    subject.OnNext((DropDownDataTypeValue, _inputFieldAge.text));
                    break;
                case DataType.User:
                    subject.OnNext((DropDownDataTypeValue, new UserData(_inputFieldName.text, uint.Parse(_inputFieldAge.text))));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}