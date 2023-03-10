using System;

namespace ETHotfix
{
    public static class ComponentFactory
    {
        public static Component CreateWithParent(Type type, Component parent)
        {
            Component component = Game.ObjectPool.Fetch(type);
            component.Parent = parent;

            Game.EventSystem.Awake(component);
            return component;
        }

        public static T CreateWithParent<T>(Component parent) where T : Component
        {
            T component = Game.ObjectPool.Fetch<T>();
            component.Parent = parent;

            Game.EventSystem.Awake(component);
            return component;
        }

        public static T CreateWithParent<T, A>(Component parent, A a) where T : Component
        {
            T component = Game.ObjectPool.Fetch<T>();
            component.Parent = parent;

            Game.EventSystem.Awake(component, a);
            return component;
        }

        public static T CreateWithParent<T, A, B>(Component parent, A a, B b) where T : Component
        {
            T component = Game.ObjectPool.Fetch<T>();
            component.Parent = parent;

            Game.EventSystem.Awake(component, a, b);
            return component;
        }

        public static T CreateWithParent<T, A, B, C>(Component parent, A a, B b, C c) where T : Component
        {
            T component = Game.ObjectPool.Fetch<T>();
            component.Parent = parent;

            Game.EventSystem.Awake(component, a, b, c);
            return component;
        }

        public static T Create<T>() where T : Component
        {
            T component = Game.ObjectPool.Fetch<T>();

            Game.EventSystem.Awake(component);
            return component;
        }

        public static Component Create(Type type)
        {
            var component = Game.ObjectPool.Fetch(type);

            Game.EventSystem.Awake(component);
            return component;
        }


        public static T Create<T, A>(A a) where T : Component
        {
            T component = Game.ObjectPool.Fetch<T>();

            Game.EventSystem.Awake(component, a);
            return component;
        }

        public static T Create<T, A, B>(A a, B b) where T : Component
        {
            T component = Game.ObjectPool.Fetch<T>();

            Game.EventSystem.Awake(component, a, b);
            return component;
        }

        public static T Create<T, A, B, C>(A a, B b, C c) where T : Component
        {
            T component = Game.ObjectPool.Fetch<T>();

            Game.EventSystem.Awake(component, a, b, c);
            return component;
        }
    }
}