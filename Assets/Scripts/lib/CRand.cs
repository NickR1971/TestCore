using UnityEngine;

//-----------------------------------------------------------------------
// класс для расчета псевдослучайных последовательностей
// используется линейный конгруэнтный метод генерации 
public class CRand
{
	private readonly ulong m;
	private readonly ulong a;
	private readonly ulong c;
	private	 ulong x;

	// значения предложенные по умолчанию показывают неплохие характеристики
	// (например величина цикла равна m), но ничего не мешает их переопределить
	// и потестировать полученную последовательность ( m = 2147483647 = 2^31-1 )
	public	CRand(uint _x0) { x=_x0; m=2147483647; a=1664525; c=1013904223; }
	public	void Reset(uint _x0) { x=_x0; }
	// возвращает целое значение в диапазоне от 1 до _d как бросок кубика
	public	uint Dice(uint _d) { return (uint)(Get()*(double)(_d))+1; }
	public int Range(int _min, int _max)
    {
//		if (_max < _min) return -1;
		if (_max < _min)
        {
			int n;
			n = _min;	_min = _max;	_max = n;
        }
		else if (_max == _min) return _min;
		return (int)Dice((uint)(_max - _min) + 1) + _min - 1;
    }
	// устанавливает случайное значение x0
	public void Randomize() { Reset((uint)((Random.value + Time.unscaledTime) * 100000000.0f)); }

//private:
	// собственно формула расчета - если надо использовать значение m
	// отличное от вида 2^n-1 то вместо побитового И надо поставить взятие остатка
	private	uint Calc() { return (uint)(x=(((a * x) + c) & m)); }
	// возвращает значение в диапазоне 0.0 до 1.0 НЕ ВКЛЮЧАЯ 1.0
	// (double!) при использовании типа float были существенные потери точности
	private	double Get() { return ((double)Calc())/(double)(m+1); }
};
