# 2017-10-11 v0.3.2

Features
- Create default Equals implementation (#95)
- Optimize clone performance (#104)
- Create base class patterns of StateElement (#103)

Bug Fixes
- Bugs of Enum selection on StateEditor (#105)
- Show uint? on StateEditor (#114)

Etc
- REST example (#94)
- Divide core logic to different folder (#111)
- Create scene transition example (#15)

# 2017-09-06 v0.3.1

Breaking Changes
- Delete IStateClone https://github.com/mattak/Unidux/issues/97

Features
- Create template for Unidux component  https://github.com/mattak/Unidux/issues/71
- Separate dependency of JsonUtility  https://github.com/mattak/Unidux/issues/88
- Editor extension to watch State object https://github.com/mattak/Unidux/issues/5

Bug Fixes
- Fix bug: Detecting field change https://github.com/mattak/Unidux/issues/91

Etc
- Update Unity version to 2017.1  https://github.com/mattak/Unidux/issues/98

# 2017-06-27 v0.3.0

Breaking Changes
- Store interface is heavily changed for supporting new features.
- Remove IRenderer, IUniduxSubscriber https://github.com/mattak/Unidux/issues/80
- IsStateChanged() => IsStateChanged https://github.com/mattak/Unidux/issues/78
- Remove generics restriction of IStateClone https://github.com/mattak/Unidux/issues/82
- Discard AddRenderer & Replace it by UniRx.Subject https://github.com/mattak/Unidux/issues/73
- Discard onetime value support https://github.com/mattak/Unidux/issues/56
- Discard Binder https://github.com/mattak/Unidux/issues/62

Features
- Support deep cloning of State object https://github.com/mattak/Unidux/issues/46
- get state interface to IStore https://github.com/mattak/Unidux/issues/72
- Simplify reducer implementation https://github.com/mattak/Unidux/issues/24
- EdtiorExtension for saving state & reloading state https://github.com/mattak/Unidux/issues/47
- Bump version to Unity 5.6.1 https://github.com/mattak/Unidux/issues/85
- Middleware support https://github.com/mattak/Unidux/issues/55

Etc
- More explanatory generic arguments in docs https://github.com/mattak/Unidux/pull/68
- Replace one character generic argument to be more explanatory https://github.com/mattak/Unidux/issues/67
- Enable syntax highlighting in docs https://github.com/mattak/Unidux/pull/65

# 2017-01-10 v0.2.2

Features
- Show warning if missing reducer https://github.com/mattak/Unidux/issues/58
- Create TodoListExample https://github.com/mattak/Unidux/issues/14

# 2016-12-24 v0.2.1

Breaking Changes
- All scripts was moved into Plugins folder https://github.com/mattak/Unidux/issues/44

Etc
- Apply [ducks proposal](https://github.com/erikras/ducks-modular-redux) to Example/{Counter,List} https://github.com/mattak/Unidux/issues/49 https://github.com/mattak/Unidux/issues/51
- Refer [UniRx](https://github.com/neuecc/UniRx) and improve Example/{Counter,List} https://github.com/mattak/Unidux/issues/52

# 2016-07-06 v0.2.0

Breaking Changes
- Remove subscribe trigger https://github.com/mattak/Unidux/issues/39
- Consistent State Rendering https://github.com/mattak/Unidux/issues/35
- Be virtual SingletonMonoBehaviour https://github.com/mattak/Unidux/issues/31
- AddTo, AddSustainTo => AddDisableTo, AddDestroyTo https://github.com/mattak/Unidux/issues/30
- move shortcut GameObject to MonoBehaviour https://github.com/mattak/Unidux/issues/29

Bug Fixes
- Multiple AddTo not works well https://github.com/mattak/Unidux/issues/33

Etc
- Update List, Count Examples to fit new Unidux.

# 2016-06-30 v0.1.1

Bug Fixes
- Add virtual keyword to SingletonMonoBehaviour Awake method https://github.com/mattak/Unidux/pull/32
- Handle multiple AddTo in same GameObject. https://github.com/mattak/Unidux/issues/33

# 2016-06-27 v0.1.0

Features
- IStateChagned property added https://github.com/mattak/Unidux/issues/22

Breaking Changes
- OneTime to FlushAfterRender https://github.com/mattak/Unidux/issues/20

Etc
- CounterExample added https://github.com/mattak/Unidux/issues/3
- ListViewExample added https://github.com/mattak/Unidux/issues/26

# 2016-06-13 v0.0.2

Bug Fixes
- Cannot unsubscribe render. https://github.com/mattak/Unidux/issues/13

Etc
- Lisence update to MIT. https://github.com/mattak/Unidux/issues/11
- Create Store test. https://github.com/mattak/Unidux/issues/10

# 2016-06-10 v0.0.1

Initial Release
