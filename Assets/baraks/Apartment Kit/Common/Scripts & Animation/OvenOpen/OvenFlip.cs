using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SojaExiles

{
	public class OvenFlip: MonoBehaviour
	{

		public Animator openandcloseoven;
		public bool open;
		public Transform Player;

		void Start()
		{
			open = false;
		}

		void OnMouseOver()
		{
			{
				if (Player)
				{
					float dist = Vector3.Distance(Player.position, transform.position);
					if (dist < 15)
					{
						if (open == false)
						{
							if (Input.GetMouseButtonDown(0))
							{
								StartCoroutine(opening());
							}
						}
						else
						{
							if (open == true)
							{
								if (Input.GetMouseButtonDown(0))
								{
									StartCoroutine(closing());
								}
							}

						}

					}
				}

			}

		}

		IEnumerator opening()
		{
			openandcloseoven.Play("OpenOven");
			open = true;
			yield return new WaitForSeconds(.5f);
		}

		IEnumerator closing()
		{
			openandcloseoven.Play("ClosingOven");
			open = false;
			yield return new WaitForSeconds(.5f);
		}


	}
}