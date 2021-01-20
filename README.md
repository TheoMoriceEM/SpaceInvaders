# SpaceInvaders
2D space-shooter game developed with Unity 2020.2.1

### Provenance des assets
Sprites des capsules et vaisseaux ennemis par Théo Morice  
Tout le reste par Richard Carlier pour le jeu Asteroids

### UML simplifié (liste des classes et de leurs méthodes) :
* tm_Bullet
    * Update()
* tm_Capsule
    * Update()
* tm_Enemy
    * Start()
    * Update()
    * tm_Fire()
    * tm_Shoot()
    * OnTriggerEnter2D()
* tm_EnemyBullet
    * Update()
* tm_GameManager
    * Start()
    * tm_LaunchGame()
    * tm_LoadLevel()
    * tm_InitGame()
    * tm_UpdateTexts()
    * tm_AddScore()
    * Update()
    * tm_EndOfLevel()
    * tm_InstantiateCapsule()
    * tm_LevelUp()
    * tm_KillPlayer()
    * tm_PlayerAgain()
    * tm_GameOver()
* tm_LimitScreen
    * Start()
    * Update()
    * tm_GameOver()
* tm_PauseManager
    * Update()
    * tm_PauseGame()
* tm_Ship
    * Start()
    * Update()
    * tm_Fire()
    * tm_Shoot()
    * tm_Move()
    * FixedUpdate()
    * tm_ManageBonus()
    * OnTriggerEnter2D()
