using System;
using UnityEngine;
using LemonFramework.ResModule;

namespace LemonFramework.UIModule
{
    /// <summary>
    /// 视图基类
    /// 根节点必须有CanvasGroup组件
    /// 属性ResInfo:设置AB包路径以及资源名,默认ResInfo(abPath="AssetBundle/[小写类名].assetbundle",assetName="[类名]",async=false)
    /// 属性CanvasInfo:设置父节点,默认CanvasInfo(type=CanvasType.FindWithTag,param="MainCanvas")
    /// </summary>
    public abstract class ViewBase<TPresenter> : IView, IResLoader
    where TPresenter : class, IPresenter
    {
        /// <summary>
        /// 根节点
        /// </summary>
        protected RectTransform _root;

        /// <summary>
        /// 根节点CanvasGroup组件
        /// </summary>
        protected CanvasGroup _rootCanvas;

        /// <summary>
        /// 交互
        /// </summary>
        protected TPresenter _presenter;

        /// <summary>
        /// 资源加载
        /// </summary>
        private IResourceManager m_ResourceManager;

        /// <summary>
        /// 资源
        /// </summary>
        private ILemonAsset _asset;

        /// <summary>
        /// 已创建标识
        /// </summary>
        private bool _created;

        /// <summary>
        /// 激活
        /// </summary>
        public bool Active
        {
            get
            {
                return _rootCanvas.IsActive();
            }
            set
            {
                _rootCanvas.SetActive(value);
            }
        }

        /// <summary>
        /// AB包加载
        /// </summary>
        public IResourceManager ResourceManager
        {
            get
            {
                if (m_ResourceManager == null)
                {
                    m_ResourceManager = UISetting.DefaultResourceManager;
                }
                return m_ResourceManager;
            }
            set
            {
                m_ResourceManager = value;
            }
        }

        /// <summary>
        /// 交互
        /// </summary>
        public IPresenter Presenter
        {
            get
            {
                return _presenter;
            }
            set
            {
                if (_presenter != null)
                {
                    _presenter.Uninstall();
                }
                _presenter = value as TPresenter;
                if (_presenter != null)
                {
                    _presenter.View = this;
                    _presenter.Install();
                }
            }
        }

        public ViewBase()
        {
            this.Presenter = Container.Resolve<TPresenter>(null);
        }

        /// <summary>
        /// 创建
        /// </summary>
        public void Create(Action callback = null)
        {
            if (!_created)
            {
                this.GetObjByResInfo((string abPath, Type type, ILemonAsset obj) =>
                {
                    if (obj == null)
                    {
                        //Log.Error($"UI Error Cls: {GetType().Name} Func:Create Info:Load res failed !  Asset Path:{obj.pathOrURL}");
                    }
                    _asset = obj;
                    var temp_obj = obj.Get<GameObject>();
                    Transform transform = ParseParentAttr();
                    _root = UnityEngine.Object.Instantiate<GameObject>(temp_obj, transform).GetComponent<RectTransform>();
                    if (_root == null)
                    {
                        //Log.Error($"UI Error Cls: {GetType().Name} Func:Create Info:Instantiate failed !");
                    }
                    _rootCanvas = _root.GetComponent<CanvasGroup>();
                    if (_rootCanvas == null)
                    {
                        _rootCanvas = _root.gameObject.AddComponent<CanvasGroup>();
                    }
                    OnCreate();
                    _created = true;
                    object tempPresenter = _presenter;
                    if (tempPresenter != null)
                    {
                        ((IPresenter)tempPresenter).OnCreateCompleted();
                    }
                    Action action = callback;
                    if (action != null)
                    {
                        action();
                    }
                }, progressCallback, UISetting.DefaultAssetLoadParam);
                return;
            }
            Action action1 = callback;
            if (action1 == null)
            {
                return;
            }
            action1();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Destroy()
        {
            this.OnDestroy();
            this.Presenter = null;
            UnityEngine.Object.DestroyImmediate(this._root.gameObject);

            IResourceManager resourceManager = ResourceManager;
            if (resourceManager == null)
            {
                return;
            }
            resourceManager.Unload(_asset);
        }

        /// <summary>
        /// 聚焦
        /// </summary>
        public void Focus()
        {
            this._rootCanvas.interactable = true;
            object obj = this._presenter;
            if (obj == null)
            {
                return;
            }
            ((IPresenter)obj).OnFocus();
        }

        /// <summary>
        /// 构建默认信息
        /// </summary>
        private void GenerateDefaultParentInfo(ref FindType type, ref string param)
        {
            if (UISetting.DefaultParentParam == null)
            {
                type = FindType.FindWithName;
                param = "Canvas";
                return;
            }
            type = UISetting.DefaultParentParam.findType;
            param = UISetting.DefaultParentParam.param;
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        /// <param name="callback"></param>
        public void Hide(Action callback = null)
        {
            TPresenter obj = _presenter;
            if (obj != null)
            {
                obj.OnHideStart();
            }

            this.OnHide(() =>
            {
                TPresenter temp_presenter = _presenter;
                if (temp_presenter != null)
                {
                    temp_presenter.OnHideCompleted();
                }
                Action action = callback;
                if (action == null)
                {
                    return;
                }
                action();
            });
        }

        /// <summary>
        /// 完成创建
        /// </summary>
        protected abstract void OnCreate();

        /// <summary>
        /// 资源加载进度
        /// </summary>
        protected virtual void progressCallback(float progress) { }

        /// <summary>
        /// 销毁中
        /// </summary>
        protected virtual void OnDestroy() { }

        /// <summary>
        /// 隐藏中
        /// </summary>
        protected virtual void OnHide(Action callback)
        {
            this.Active = false;
            if (callback != null)
            {
                callback();
            }
        }

        /// <summary>
        /// 显示中
        /// </summary>
        protected virtual void OnShow(Action callback)
        {
            this.Active = true;
            if (callback != null)
            {
                callback();
            }
        }

        /// <summary>
        /// 解析父节点属性
        /// </summary>
        private Transform ParseParentAttr()
        {
            Transform transform = null;
            FindType findType = FindType.None;
            string empty = string.Empty;
            this.GenerateDefaultParentInfo(ref findType, ref empty);
            object[] customAttributes = this.GetType().GetCustomAttributes(typeof(ParentInfoAttribute), true);
            if (customAttributes != null)
            {
                object[] objArray = customAttributes;
                for (int i = 0; i < objArray.Length; i++)
                {
                    ParentInfoAttribute parentInfoAttribute = (ParentInfoAttribute)objArray[i];
                    if (parentInfoAttribute != null)
                    {
                        findType = parentInfoAttribute.type;
                        empty = parentInfoAttribute.param;
                    }
                }
            }
            if (findType == FindType.FindWithTag)
            {
                transform = NodeContainer.FindNodeWithTag(empty);
            }
            else if (findType == FindType.FindWithName)
            {
                transform = NodeContainer.FindNodeWithName(empty);
            }
            return transform;
        }

        /// <summary>
        /// 显示
        /// </summary>
        public void Show(Action callback = null)
        {
            if (_presenter != null)
            {
                _presenter.OnShowStart();
            }
            this.OnShow(() =>
            {
                var temp_presenter = this._presenter;
                if (temp_presenter != null)
                {
                    temp_presenter.OnShowCompleted();
                }
                Action action = callback;
                if (action == null)
                {
                    return;
                }
                action();
            });
        }

        /// <summary>
        /// 失焦
        /// </summary>
        public void UnFocus()
        {
            this._rootCanvas.interactable = false;
            if (_presenter == null)
            {
                return;
            }
            _presenter.OnUnFocus();
        }
    }
}