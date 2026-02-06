
## Struct of Array

Basic Definition

Add

Remove

Modify

Query

Limitation

Time Complexity

## Bezier Curve

Definition:

	点的数量最低为2。
	precision 最低为3。

private field:

	control_points: Vector3Array<double>
	temp_de_Casteljau_triangle:
		precision个Vector3Array。
		由于List理应存储class指针，所以Vector3Array中的分量list将会分散在内存中不同位置。
		这应该有助于cpu预取，不会造成false sharing。
		由于decimal占用128bit，不支持simd，所以使用double。

public field:

	precision: int = 3

Methods:

	data
		Add/Remove Last Point.
		Swap two points by indices.
		Update data of a point.
	functionality
		Lerp_de_Casteljau_X/Y/Z
			使用方式：分别对X/Y/Z分量进行for loop，理论上性能最好。
			分开使用场景：如果点的变化仅在某一坐标轴上平移 -> 不需要重新计算其它分量。
			合并使用场景：任意。
Plan:

	空间：
		expand/resize：拓展空间/重新分配空间
	时间：
		Lerp_de_Casteljau_X/Y/Z: 可以进行SIMD+多线程优化。
		目前数据结构已支持，但具体算法和线程池待确定。

##