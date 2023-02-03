TileManager: (Monobehaviour) -> Singleton
- 2D Array aus "Tile"
- GetTileAtPosition(Vector3 position)
- SetTileAtPosition(Vector3 position, Tile tile)


Tile: (Monobehaviour)
- Wasser, Boden oder Blatt
(Wasser hat collider, mindestens die angrenzenden an erde)
Jedes Tile hat ein "Building"

Building (Monobehaviour):
- Weiß sein eigenes Tile auf dem es steht
- Kann auf Tick basis Zeugs tun



TickManager: (Monobehaviour) -> Singleton
- BuildingTick() -> alle Buildings ticken lassen
- WaterDamageTick() -> alle Tiles mit Wasser ticken lassen
- UpkeepTick() -> alle Buildings Upkeep berechnen lassen

