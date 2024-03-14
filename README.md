# Общая информация

- Клон agar.io
- Версия Unity - 2022.3.10f1
- Время выполнения - 18 часов
- Использованные фреймворки - VContainer, Entitas, UniRx

# Реализованные пункты т/з

- Использование DI и ECS - проект выстроен по сервисной архитектуре с использованием VContainer как DI-решения и 
Entitas как ECS-фреймворка. Вся логика симуляции реализована в системах ECS - GameObject'ы используются только для
визуализации. Использование callback-методов движка сведено к единичным случаям.
- Наличие двух сцен - сцена Menu выполняет задачи Bootstrap-сцены и отображения главного меню. В главном меню имеются
три кнопки, начинающая игру, открывающая окно настроек и открывающая историю завершённых игровых сессий соответственно.
- Наличие звукового сопровождения в главном меню и игровой сессии.
- Вся сохраняемая информация сохраняется на диск в формате JSON. Также реализовано сохранение данных в PlayerPrefs в
целях демонстрации сильных сторон сервисной архитектуры.
- Реализовано окно Leaderboard, отражающее прогресс игровой сессии. Окно обновляется в реальном времени.
- Реализован ИИ противников, состоящий из четырёх основных состояний. Состояния выглядят следующим образом в
порядке приоритетности:
    - Escape - рядом есть игровая сущность с большим количеством очков - движение в противоположную от неё сторону
    - Hunt - рядом есть игровая сущность с меньшим количеством очков - движение за ней
    - Feed - рядом есть еда - движение к ней
    - Wander - рядом нет ничего из вышеперечисленного - движение в случайном направлении

# Детали технической реализации

  ## Flow логики
  - Явный контроль за состояниями игры реализован с помощью [машины состояний](Assets/Content/Infrastructure/States.meta). В проекте используются следующие состояния:
    - Bootstrap - точка входа в игру
    - Load Progress - загрузка существующих или создание новых игровых данных
    - Load Meta - создание и конфигурация главного меню
    - Load Level - создание игровой симуляции
    - Game Loop - процесс игровой симуляции
    - Game Over - конец игровой сессии, переход к одному из предыдущих состояний
  
  ## Использованные сервисы
  - Для явного разделения логических зон ответственности в проекте используются следующие [сервисы](Assets/Content/Infrastructure/Services):
    - Persistent Data - хранение игровых данных
    - Save/Load - сохранение и загрузка игровых данных
    - Input - обработка пользовательского ввода
    - Logging - логгирование
    - Gameplay - обработка логики, связанной с игровой сессией
  
  ## Прочее
  - Для переиспользования объектов представления игровых сущностей реализован [pooling](Assets/Content/Gameplay/GameplayObjectPool.cs), использующийся, в частности, для еды и врагов

# Итог
- Реализованы все основные требования из т/з
- Реализованы следующие доп. пункты т/з со всеми перечисленными требованиями:
  - Окно просмотра результатов в главном меню
  - Продвинутый ИИ противников
  - Таблица лидеров игровой сессии