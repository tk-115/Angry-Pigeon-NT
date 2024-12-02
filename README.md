# Angry-Pigeon-NT
A small mobile game about pigeon who love bombarding civilians. Version with store and further support

Оболочка игры работает при помощи паттерна состояние. Вего GameStates 3 - главное меню, магазин и геймлпей
![1](https://github.com/user-attachments/assets/5f5d2bc3-daea-44c9-b9db-85cdbadc3cf2)
В главном меню можно включить/выключить SFX и музыку, изменить язык, видеть рекорд очков, пролеченных метров и собранных монет.

![image](https://github.com/user-attachments/assets/324045e4-0707-47c1-abde-80dc6a84f9e0)
В зависимости от полученного рекорда, открываются новые голуби, которые имеют уникаьную атаку.

- Обычный скидывает бомбу
- Тройной скидывает их три впереди себя
- Разрывной скидывает бомбу, которая взрывается и поражает противников в радиусе
- Наводящийся скидывает бомбу, которая ищет противника в радиусе и летит к нему, после чего гремит взрыв, который поражает противников в радиусе.

Геймплей
![image](https://github.com/user-attachments/assets/79337cb4-9bc1-479e-853e-c7ee49a2594c)

Среди противников имеются:
- Пешеход (мужской и женский View, технически ничем не отличаются кроме очков за поражение)
- Рогатчик (стреляет 4 камнями в сторону голубя при его приближении - поражение на 25 хп)
- ПВО (стреляет 1 ракетой в сторону голубя - полное поражение)
- Ворона (летит справа налево в сторону голубя всегда из случайной точки)

![image](https://github.com/user-attachments/assets/7dbb743a-c40d-4efb-a093-f6fa40f4c2ea)

Игровой мир состоит из блоков, которые предварительно инстанциируются и инициализируются в специальный пул блоков и в процессе геймплея
рандомно вытаскиваются и используются.
В каждом блоке есть (или могут быть (для не предсказуемости геймлпея для игрока)) 
- Точки следования пешеходов - PedestrianPaths
- Спавнер бонусов
- Точки спавна рогатчика
- Точки спавна ПВО

![image](https://github.com/user-attachments/assets/b76c4d48-b6a0-4957-a84e-e72fec73485d)

После запуска геймлпея игра запускает новую машину состояния - GameplayStateMachine, которая состоит из следующих состояний:
- Геймплей
- Пауза
- Игра окончена

При запуске геймлпея спавнится голубь, у которого есть своя машина состояния - PigeonStateMachine со своими состояниями:
- IDLE
- Мертв
- Получает урон
- Летит

Бонусы
Реализовано 4 бонуса, влияющие на геймлпей:
- Множитель (удваивает получаемые очки и монеты)
- Защита (делает бессмертным голубя)
- Замедление (замедляет постоянно ускоряющееся движение голубя по уровню)
- Полное здоровье
И один бонус, не влияющий на геймплей - монета. Он спавнится рандомно в указанных точках спавна.
Для реализации был использован паттерн шаблонный метод, который благодаря полиморфизму применяет нужную логику в зависимости
от поднятого голубем бонуса.

![image](https://github.com/user-attachments/assets/ecab954b-cd32-4330-95ee-84a6a0bcf1bf)






