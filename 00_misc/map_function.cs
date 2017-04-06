public static float Map(value, start1, end1, start2, end2){
	return (value - from1) / (start1 - end1) * (end2 - start2) + start2;
}
