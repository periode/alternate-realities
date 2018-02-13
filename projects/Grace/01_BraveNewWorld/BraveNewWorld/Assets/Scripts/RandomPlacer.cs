using UnityEngine;

public class RandomPlacer : PipeItemGenerator {

	public PipeItem[] itemPrefabs;

	public override void GenerateItems (Pipe pipe) {
		float angleStep = pipe.CurveAngle / pipe.CurveSegmentCount;
		for (int i = 0; i < pipe.CurveSegmentCount; i++) {
			PipeItem item = Instantiate<PipeItem>(
				itemPrefabs[Random.Range(0, itemPrefabs.Length)]);
			float pipeRotation =
				(Random.Range(0, pipe.pipeSegmentCount) + 0.5f) *
				360f / pipe.pipeSegmentCount;
			item.Position(pipe, i * angleStep, pipeRotation);
		}
	}
}