using System;
using System.IO;
using System.Runtime.InteropServices;

// Token: 0x0200008D RID: 141
internal class SDPCorePINVOKE
{
	// Token: 0x06000A16 RID: 2582
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_Clear")]
	public static extern void ProviderList_Clear(HandleRef jarg1);

	// Token: 0x06000A17 RID: 2583
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_Add")]
	public static extern void ProviderList_Add(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000A18 RID: 2584
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_size")]
	public static extern uint ProviderList_size(HandleRef jarg1);

	// Token: 0x06000A19 RID: 2585
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_capacity")]
	public static extern uint ProviderList_capacity(HandleRef jarg1);

	// Token: 0x06000A1A RID: 2586
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_reserve")]
	public static extern void ProviderList_reserve(HandleRef jarg1, uint jarg2);

	// Token: 0x06000A1B RID: 2587
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProviderList__SWIG_0")]
	public static extern IntPtr new_ProviderList__SWIG_0();

	// Token: 0x06000A1C RID: 2588
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProviderList__SWIG_1")]
	public static extern IntPtr new_ProviderList__SWIG_1(HandleRef jarg1);

	// Token: 0x06000A1D RID: 2589
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProviderList__SWIG_2")]
	public static extern IntPtr new_ProviderList__SWIG_2(int jarg1);

	// Token: 0x06000A1E RID: 2590
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_getitemcopy")]
	public static extern IntPtr ProviderList_getitemcopy(HandleRef jarg1, int jarg2);

	// Token: 0x06000A1F RID: 2591
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_getitem")]
	public static extern IntPtr ProviderList_getitem(HandleRef jarg1, int jarg2);

	// Token: 0x06000A20 RID: 2592
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_setitem")]
	public static extern void ProviderList_setitem(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A21 RID: 2593
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_AddRange")]
	public static extern void ProviderList_AddRange(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000A22 RID: 2594
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_GetRange")]
	public static extern IntPtr ProviderList_GetRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000A23 RID: 2595
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_Insert")]
	public static extern void ProviderList_Insert(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A24 RID: 2596
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_InsertRange")]
	public static extern void ProviderList_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A25 RID: 2597
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_RemoveAt")]
	public static extern void ProviderList_RemoveAt(HandleRef jarg1, int jarg2);

	// Token: 0x06000A26 RID: 2598
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_RemoveRange")]
	public static extern void ProviderList_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000A27 RID: 2599
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_Repeat")]
	public static extern IntPtr ProviderList_Repeat(HandleRef jarg1, int jarg2);

	// Token: 0x06000A28 RID: 2600
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_Reverse__SWIG_0")]
	public static extern void ProviderList_Reverse__SWIG_0(HandleRef jarg1);

	// Token: 0x06000A29 RID: 2601
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_Reverse__SWIG_1")]
	public static extern void ProviderList_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000A2A RID: 2602
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_SetRange")]
	public static extern void ProviderList_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A2B RID: 2603
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_Contains")]
	public static extern bool ProviderList_Contains(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000A2C RID: 2604
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_IndexOf")]
	public static extern int ProviderList_IndexOf(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000A2D RID: 2605
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_LastIndexOf")]
	public static extern int ProviderList_LastIndexOf(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000A2E RID: 2606
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderList_Remove")]
	public static extern bool ProviderList_Remove(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000A2F RID: 2607
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ProviderList")]
	public static extern void delete_ProviderList(HandleRef jarg1);

	// Token: 0x06000A30 RID: 2608
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_Clear")]
	public static extern void ProcessList_Clear(HandleRef jarg1);

	// Token: 0x06000A31 RID: 2609
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_Add")]
	public static extern void ProcessList_Add(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000A32 RID: 2610
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_size")]
	public static extern uint ProcessList_size(HandleRef jarg1);

	// Token: 0x06000A33 RID: 2611
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_capacity")]
	public static extern uint ProcessList_capacity(HandleRef jarg1);

	// Token: 0x06000A34 RID: 2612
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_reserve")]
	public static extern void ProcessList_reserve(HandleRef jarg1, uint jarg2);

	// Token: 0x06000A35 RID: 2613
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessList__SWIG_0")]
	public static extern IntPtr new_ProcessList__SWIG_0();

	// Token: 0x06000A36 RID: 2614
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessList__SWIG_1")]
	public static extern IntPtr new_ProcessList__SWIG_1(HandleRef jarg1);

	// Token: 0x06000A37 RID: 2615
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessList__SWIG_2")]
	public static extern IntPtr new_ProcessList__SWIG_2(int jarg1);

	// Token: 0x06000A38 RID: 2616
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_getitemcopy")]
	public static extern IntPtr ProcessList_getitemcopy(HandleRef jarg1, int jarg2);

	// Token: 0x06000A39 RID: 2617
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_getitem")]
	public static extern IntPtr ProcessList_getitem(HandleRef jarg1, int jarg2);

	// Token: 0x06000A3A RID: 2618
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_setitem")]
	public static extern void ProcessList_setitem(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A3B RID: 2619
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_AddRange")]
	public static extern void ProcessList_AddRange(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000A3C RID: 2620
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_GetRange")]
	public static extern IntPtr ProcessList_GetRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000A3D RID: 2621
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_Insert")]
	public static extern void ProcessList_Insert(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A3E RID: 2622
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_InsertRange")]
	public static extern void ProcessList_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A3F RID: 2623
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_RemoveAt")]
	public static extern void ProcessList_RemoveAt(HandleRef jarg1, int jarg2);

	// Token: 0x06000A40 RID: 2624
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_RemoveRange")]
	public static extern void ProcessList_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000A41 RID: 2625
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_Repeat")]
	public static extern IntPtr ProcessList_Repeat(HandleRef jarg1, int jarg2);

	// Token: 0x06000A42 RID: 2626
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_Reverse__SWIG_0")]
	public static extern void ProcessList_Reverse__SWIG_0(HandleRef jarg1);

	// Token: 0x06000A43 RID: 2627
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_Reverse__SWIG_1")]
	public static extern void ProcessList_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000A44 RID: 2628
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessList_SetRange")]
	public static extern void ProcessList_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A45 RID: 2629
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ProcessList")]
	public static extern void delete_ProcessList(HandleRef jarg1);

	// Token: 0x06000A46 RID: 2630
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_Clear")]
	public static extern void IDList_Clear(HandleRef jarg1);

	// Token: 0x06000A47 RID: 2631
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_Add")]
	public static extern void IDList_Add(HandleRef jarg1, uint jarg2);

	// Token: 0x06000A48 RID: 2632
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_size")]
	public static extern uint IDList_size(HandleRef jarg1);

	// Token: 0x06000A49 RID: 2633
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_capacity")]
	public static extern uint IDList_capacity(HandleRef jarg1);

	// Token: 0x06000A4A RID: 2634
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_reserve")]
	public static extern void IDList_reserve(HandleRef jarg1, uint jarg2);

	// Token: 0x06000A4B RID: 2635
	[DllImport("SDPCore", EntryPoint = "CSharp_new_IDList__SWIG_0")]
	public static extern IntPtr new_IDList__SWIG_0();

	// Token: 0x06000A4C RID: 2636
	[DllImport("SDPCore", EntryPoint = "CSharp_new_IDList__SWIG_1")]
	public static extern IntPtr new_IDList__SWIG_1(HandleRef jarg1);

	// Token: 0x06000A4D RID: 2637
	[DllImport("SDPCore", EntryPoint = "CSharp_new_IDList__SWIG_2")]
	public static extern IntPtr new_IDList__SWIG_2(int jarg1);

	// Token: 0x06000A4E RID: 2638
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_getitemcopy")]
	public static extern uint IDList_getitemcopy(HandleRef jarg1, int jarg2);

	// Token: 0x06000A4F RID: 2639
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_getitem")]
	public static extern uint IDList_getitem(HandleRef jarg1, int jarg2);

	// Token: 0x06000A50 RID: 2640
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_setitem")]
	public static extern void IDList_setitem(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x06000A51 RID: 2641
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_AddRange")]
	public static extern void IDList_AddRange(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000A52 RID: 2642
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_GetRange")]
	public static extern IntPtr IDList_GetRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000A53 RID: 2643
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_Insert")]
	public static extern void IDList_Insert(HandleRef jarg1, int jarg2, uint jarg3);

	// Token: 0x06000A54 RID: 2644
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_InsertRange")]
	public static extern void IDList_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A55 RID: 2645
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_RemoveAt")]
	public static extern void IDList_RemoveAt(HandleRef jarg1, int jarg2);

	// Token: 0x06000A56 RID: 2646
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_RemoveRange")]
	public static extern void IDList_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000A57 RID: 2647
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_Repeat")]
	public static extern IntPtr IDList_Repeat(uint jarg1, int jarg2);

	// Token: 0x06000A58 RID: 2648
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_Reverse__SWIG_0")]
	public static extern void IDList_Reverse__SWIG_0(HandleRef jarg1);

	// Token: 0x06000A59 RID: 2649
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_Reverse__SWIG_1")]
	public static extern void IDList_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000A5A RID: 2650
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_SetRange")]
	public static extern void IDList_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A5B RID: 2651
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_Contains")]
	public static extern bool IDList_Contains(HandleRef jarg1, uint jarg2);

	// Token: 0x06000A5C RID: 2652
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_IndexOf")]
	public static extern int IDList_IndexOf(HandleRef jarg1, uint jarg2);

	// Token: 0x06000A5D RID: 2653
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_LastIndexOf")]
	public static extern int IDList_LastIndexOf(HandleRef jarg1, uint jarg2);

	// Token: 0x06000A5E RID: 2654
	[DllImport("SDPCore", EntryPoint = "CSharp_IDList_Remove")]
	public static extern bool IDList_Remove(HandleRef jarg1, uint jarg2);

	// Token: 0x06000A5F RID: 2655
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_IDList")]
	public static extern void delete_IDList(HandleRef jarg1);

	// Token: 0x06000A60 RID: 2656
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_Clear")]
	public static extern void MetricList_Clear(HandleRef jarg1);

	// Token: 0x06000A61 RID: 2657
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_Add")]
	public static extern void MetricList_Add(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000A62 RID: 2658
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_size")]
	public static extern uint MetricList_size(HandleRef jarg1);

	// Token: 0x06000A63 RID: 2659
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_capacity")]
	public static extern uint MetricList_capacity(HandleRef jarg1);

	// Token: 0x06000A64 RID: 2660
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_reserve")]
	public static extern void MetricList_reserve(HandleRef jarg1, uint jarg2);

	// Token: 0x06000A65 RID: 2661
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricList__SWIG_0")]
	public static extern IntPtr new_MetricList__SWIG_0();

	// Token: 0x06000A66 RID: 2662
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricList__SWIG_1")]
	public static extern IntPtr new_MetricList__SWIG_1(HandleRef jarg1);

	// Token: 0x06000A67 RID: 2663
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricList__SWIG_2")]
	public static extern IntPtr new_MetricList__SWIG_2(int jarg1);

	// Token: 0x06000A68 RID: 2664
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_getitemcopy")]
	public static extern IntPtr MetricList_getitemcopy(HandleRef jarg1, int jarg2);

	// Token: 0x06000A69 RID: 2665
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_getitem")]
	public static extern IntPtr MetricList_getitem(HandleRef jarg1, int jarg2);

	// Token: 0x06000A6A RID: 2666
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_setitem")]
	public static extern void MetricList_setitem(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A6B RID: 2667
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_AddRange")]
	public static extern void MetricList_AddRange(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000A6C RID: 2668
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_GetRange")]
	public static extern IntPtr MetricList_GetRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000A6D RID: 2669
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_Insert")]
	public static extern void MetricList_Insert(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A6E RID: 2670
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_InsertRange")]
	public static extern void MetricList_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A6F RID: 2671
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_RemoveAt")]
	public static extern void MetricList_RemoveAt(HandleRef jarg1, int jarg2);

	// Token: 0x06000A70 RID: 2672
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_RemoveRange")]
	public static extern void MetricList_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000A71 RID: 2673
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_Repeat")]
	public static extern IntPtr MetricList_Repeat(HandleRef jarg1, int jarg2);

	// Token: 0x06000A72 RID: 2674
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_Reverse__SWIG_0")]
	public static extern void MetricList_Reverse__SWIG_0(HandleRef jarg1);

	// Token: 0x06000A73 RID: 2675
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_Reverse__SWIG_1")]
	public static extern void MetricList_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000A74 RID: 2676
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricList_SetRange")]
	public static extern void MetricList_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A75 RID: 2677
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_MetricList")]
	public static extern void delete_MetricList(HandleRef jarg1);

	// Token: 0x06000A76 RID: 2678
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricIDList__SWIG_0")]
	public static extern IntPtr new_MetricIDList__SWIG_0();

	// Token: 0x06000A77 RID: 2679
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricIDList__SWIG_1")]
	public static extern IntPtr new_MetricIDList__SWIG_1(HandleRef jarg1);

	// Token: 0x06000A78 RID: 2680
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricIDList_size")]
	public static extern uint MetricIDList_size(HandleRef jarg1);

	// Token: 0x06000A79 RID: 2681
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricIDList_empty")]
	public static extern bool MetricIDList_empty(HandleRef jarg1);

	// Token: 0x06000A7A RID: 2682
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricIDList_clear")]
	public static extern void MetricIDList_clear(HandleRef jarg1);

	// Token: 0x06000A7B RID: 2683
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricIDList_Contains")]
	public static extern bool MetricIDList_Contains(HandleRef jarg1, uint jarg2);

	// Token: 0x06000A7C RID: 2684
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricIDList_Add")]
	public static extern void MetricIDList_Add(HandleRef jarg1, uint jarg2);

	// Token: 0x06000A7D RID: 2685
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricIDList_Remove")]
	public static extern bool MetricIDList_Remove(HandleRef jarg1, uint jarg2);

	// Token: 0x06000A7E RID: 2686
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricIDList_create_iterator_begin")]
	public static extern IntPtr MetricIDList_create_iterator_begin(HandleRef jarg1);

	// Token: 0x06000A7F RID: 2687
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricIDList_get_next")]
	public static extern uint MetricIDList_get_next(HandleRef jarg1, IntPtr jarg2);

	// Token: 0x06000A80 RID: 2688
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricIDList_destroy_iterator")]
	public static extern void MetricIDList_destroy_iterator(HandleRef jarg1, IntPtr jarg2);

	// Token: 0x06000A81 RID: 2689
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_MetricIDList")]
	public static extern void delete_MetricIDList(HandleRef jarg1);

	// Token: 0x06000A82 RID: 2690
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_Clear")]
	public static extern void MetricCategoryList_Clear(HandleRef jarg1);

	// Token: 0x06000A83 RID: 2691
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_Add")]
	public static extern void MetricCategoryList_Add(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000A84 RID: 2692
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_size")]
	public static extern uint MetricCategoryList_size(HandleRef jarg1);

	// Token: 0x06000A85 RID: 2693
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_capacity")]
	public static extern uint MetricCategoryList_capacity(HandleRef jarg1);

	// Token: 0x06000A86 RID: 2694
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_reserve")]
	public static extern void MetricCategoryList_reserve(HandleRef jarg1, uint jarg2);

	// Token: 0x06000A87 RID: 2695
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricCategoryList__SWIG_0")]
	public static extern IntPtr new_MetricCategoryList__SWIG_0();

	// Token: 0x06000A88 RID: 2696
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricCategoryList__SWIG_1")]
	public static extern IntPtr new_MetricCategoryList__SWIG_1(HandleRef jarg1);

	// Token: 0x06000A89 RID: 2697
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricCategoryList__SWIG_2")]
	public static extern IntPtr new_MetricCategoryList__SWIG_2(int jarg1);

	// Token: 0x06000A8A RID: 2698
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_getitemcopy")]
	public static extern IntPtr MetricCategoryList_getitemcopy(HandleRef jarg1, int jarg2);

	// Token: 0x06000A8B RID: 2699
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_getitem")]
	public static extern IntPtr MetricCategoryList_getitem(HandleRef jarg1, int jarg2);

	// Token: 0x06000A8C RID: 2700
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_setitem")]
	public static extern void MetricCategoryList_setitem(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A8D RID: 2701
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_AddRange")]
	public static extern void MetricCategoryList_AddRange(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000A8E RID: 2702
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_GetRange")]
	public static extern IntPtr MetricCategoryList_GetRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000A8F RID: 2703
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_Insert")]
	public static extern void MetricCategoryList_Insert(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A90 RID: 2704
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_InsertRange")]
	public static extern void MetricCategoryList_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A91 RID: 2705
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_RemoveAt")]
	public static extern void MetricCategoryList_RemoveAt(HandleRef jarg1, int jarg2);

	// Token: 0x06000A92 RID: 2706
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_RemoveRange")]
	public static extern void MetricCategoryList_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000A93 RID: 2707
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_Repeat")]
	public static extern IntPtr MetricCategoryList_Repeat(HandleRef jarg1, int jarg2);

	// Token: 0x06000A94 RID: 2708
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_Reverse__SWIG_0")]
	public static extern void MetricCategoryList_Reverse__SWIG_0(HandleRef jarg1);

	// Token: 0x06000A95 RID: 2709
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_Reverse__SWIG_1")]
	public static extern void MetricCategoryList_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000A96 RID: 2710
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryList_SetRange")]
	public static extern void MetricCategoryList_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000A97 RID: 2711
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_MetricCategoryList")]
	public static extern void delete_MetricCategoryList(HandleRef jarg1);

	// Token: 0x06000A98 RID: 2712
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_Clear")]
	public static extern void ThreadList_Clear(HandleRef jarg1);

	// Token: 0x06000A99 RID: 2713
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_Add")]
	public static extern void ThreadList_Add(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000A9A RID: 2714
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_size")]
	public static extern uint ThreadList_size(HandleRef jarg1);

	// Token: 0x06000A9B RID: 2715
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_capacity")]
	public static extern uint ThreadList_capacity(HandleRef jarg1);

	// Token: 0x06000A9C RID: 2716
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_reserve")]
	public static extern void ThreadList_reserve(HandleRef jarg1, uint jarg2);

	// Token: 0x06000A9D RID: 2717
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ThreadList__SWIG_0")]
	public static extern IntPtr new_ThreadList__SWIG_0();

	// Token: 0x06000A9E RID: 2718
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ThreadList__SWIG_1")]
	public static extern IntPtr new_ThreadList__SWIG_1(HandleRef jarg1);

	// Token: 0x06000A9F RID: 2719
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ThreadList__SWIG_2")]
	public static extern IntPtr new_ThreadList__SWIG_2(int jarg1);

	// Token: 0x06000AA0 RID: 2720
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_getitemcopy")]
	public static extern IntPtr ThreadList_getitemcopy(HandleRef jarg1, int jarg2);

	// Token: 0x06000AA1 RID: 2721
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_getitem")]
	public static extern IntPtr ThreadList_getitem(HandleRef jarg1, int jarg2);

	// Token: 0x06000AA2 RID: 2722
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_setitem")]
	public static extern void ThreadList_setitem(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000AA3 RID: 2723
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_AddRange")]
	public static extern void ThreadList_AddRange(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000AA4 RID: 2724
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_GetRange")]
	public static extern IntPtr ThreadList_GetRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000AA5 RID: 2725
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_Insert")]
	public static extern void ThreadList_Insert(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000AA6 RID: 2726
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_InsertRange")]
	public static extern void ThreadList_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000AA7 RID: 2727
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_RemoveAt")]
	public static extern void ThreadList_RemoveAt(HandleRef jarg1, int jarg2);

	// Token: 0x06000AA8 RID: 2728
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_RemoveRange")]
	public static extern void ThreadList_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000AA9 RID: 2729
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_Repeat")]
	public static extern IntPtr ThreadList_Repeat(HandleRef jarg1, int jarg2);

	// Token: 0x06000AAA RID: 2730
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_Reverse__SWIG_0")]
	public static extern void ThreadList_Reverse__SWIG_0(HandleRef jarg1);

	// Token: 0x06000AAB RID: 2731
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_Reverse__SWIG_1")]
	public static extern void ThreadList_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000AAC RID: 2732
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_SetRange")]
	public static extern void ThreadList_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000AAD RID: 2733
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_Contains")]
	public static extern bool ThreadList_Contains(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000AAE RID: 2734
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_IndexOf")]
	public static extern int ThreadList_IndexOf(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000AAF RID: 2735
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_LastIndexOf")]
	public static extern int ThreadList_LastIndexOf(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000AB0 RID: 2736
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadList_Remove")]
	public static extern bool ThreadList_Remove(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000AB1 RID: 2737
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ThreadList")]
	public static extern void delete_ThreadList(HandleRef jarg1);

	// Token: 0x06000AB2 RID: 2738
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_Clear")]
	public static extern void DeviceList_Clear(HandleRef jarg1);

	// Token: 0x06000AB3 RID: 2739
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_Add")]
	public static extern void DeviceList_Add(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000AB4 RID: 2740
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_size")]
	public static extern uint DeviceList_size(HandleRef jarg1);

	// Token: 0x06000AB5 RID: 2741
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_capacity")]
	public static extern uint DeviceList_capacity(HandleRef jarg1);

	// Token: 0x06000AB6 RID: 2742
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_reserve")]
	public static extern void DeviceList_reserve(HandleRef jarg1, uint jarg2);

	// Token: 0x06000AB7 RID: 2743
	[DllImport("SDPCore", EntryPoint = "CSharp_new_DeviceList__SWIG_0")]
	public static extern IntPtr new_DeviceList__SWIG_0();

	// Token: 0x06000AB8 RID: 2744
	[DllImport("SDPCore", EntryPoint = "CSharp_new_DeviceList__SWIG_1")]
	public static extern IntPtr new_DeviceList__SWIG_1(HandleRef jarg1);

	// Token: 0x06000AB9 RID: 2745
	[DllImport("SDPCore", EntryPoint = "CSharp_new_DeviceList__SWIG_2")]
	public static extern IntPtr new_DeviceList__SWIG_2(int jarg1);

	// Token: 0x06000ABA RID: 2746
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_getitemcopy")]
	public static extern IntPtr DeviceList_getitemcopy(HandleRef jarg1, int jarg2);

	// Token: 0x06000ABB RID: 2747
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_getitem")]
	public static extern IntPtr DeviceList_getitem(HandleRef jarg1, int jarg2);

	// Token: 0x06000ABC RID: 2748
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_setitem")]
	public static extern void DeviceList_setitem(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000ABD RID: 2749
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_AddRange")]
	public static extern void DeviceList_AddRange(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000ABE RID: 2750
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_GetRange")]
	public static extern IntPtr DeviceList_GetRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000ABF RID: 2751
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_Insert")]
	public static extern void DeviceList_Insert(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000AC0 RID: 2752
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_InsertRange")]
	public static extern void DeviceList_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000AC1 RID: 2753
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_RemoveAt")]
	public static extern void DeviceList_RemoveAt(HandleRef jarg1, int jarg2);

	// Token: 0x06000AC2 RID: 2754
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_RemoveRange")]
	public static extern void DeviceList_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000AC3 RID: 2755
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_Repeat")]
	public static extern IntPtr DeviceList_Repeat(HandleRef jarg1, int jarg2);

	// Token: 0x06000AC4 RID: 2756
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_Reverse__SWIG_0")]
	public static extern void DeviceList_Reverse__SWIG_0(HandleRef jarg1);

	// Token: 0x06000AC5 RID: 2757
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_Reverse__SWIG_1")]
	public static extern void DeviceList_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000AC6 RID: 2758
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_SetRange")]
	public static extern void DeviceList_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000AC7 RID: 2759
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_Contains")]
	public static extern bool DeviceList_Contains(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000AC8 RID: 2760
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_IndexOf")]
	public static extern int DeviceList_IndexOf(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000AC9 RID: 2761
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_LastIndexOf")]
	public static extern int DeviceList_LastIndexOf(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000ACA RID: 2762
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceList_Remove")]
	public static extern bool DeviceList_Remove(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000ACB RID: 2763
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_DeviceList")]
	public static extern void delete_DeviceList(HandleRef jarg1);

	// Token: 0x06000ACC RID: 2764
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_Clear")]
	public static extern void OptionList_Clear(HandleRef jarg1);

	// Token: 0x06000ACD RID: 2765
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_Add")]
	public static extern void OptionList_Add(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000ACE RID: 2766
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_size")]
	public static extern uint OptionList_size(HandleRef jarg1);

	// Token: 0x06000ACF RID: 2767
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_capacity")]
	public static extern uint OptionList_capacity(HandleRef jarg1);

	// Token: 0x06000AD0 RID: 2768
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_reserve")]
	public static extern void OptionList_reserve(HandleRef jarg1, uint jarg2);

	// Token: 0x06000AD1 RID: 2769
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionList__SWIG_0")]
	public static extern IntPtr new_OptionList__SWIG_0();

	// Token: 0x06000AD2 RID: 2770
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionList__SWIG_1")]
	public static extern IntPtr new_OptionList__SWIG_1(HandleRef jarg1);

	// Token: 0x06000AD3 RID: 2771
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionList__SWIG_2")]
	public static extern IntPtr new_OptionList__SWIG_2(int jarg1);

	// Token: 0x06000AD4 RID: 2772
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_getitemcopy")]
	public static extern IntPtr OptionList_getitemcopy(HandleRef jarg1, int jarg2);

	// Token: 0x06000AD5 RID: 2773
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_getitem")]
	public static extern IntPtr OptionList_getitem(HandleRef jarg1, int jarg2);

	// Token: 0x06000AD6 RID: 2774
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_setitem")]
	public static extern void OptionList_setitem(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000AD7 RID: 2775
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_AddRange")]
	public static extern void OptionList_AddRange(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000AD8 RID: 2776
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_GetRange")]
	public static extern IntPtr OptionList_GetRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000AD9 RID: 2777
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_Insert")]
	public static extern void OptionList_Insert(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000ADA RID: 2778
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_InsertRange")]
	public static extern void OptionList_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000ADB RID: 2779
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_RemoveAt")]
	public static extern void OptionList_RemoveAt(HandleRef jarg1, int jarg2);

	// Token: 0x06000ADC RID: 2780
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_RemoveRange")]
	public static extern void OptionList_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000ADD RID: 2781
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_Repeat")]
	public static extern IntPtr OptionList_Repeat(HandleRef jarg1, int jarg2);

	// Token: 0x06000ADE RID: 2782
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_Reverse__SWIG_0")]
	public static extern void OptionList_Reverse__SWIG_0(HandleRef jarg1);

	// Token: 0x06000ADF RID: 2783
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_Reverse__SWIG_1")]
	public static extern void OptionList_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000AE0 RID: 2784
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_SetRange")]
	public static extern void OptionList_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000AE1 RID: 2785
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_Contains")]
	public static extern bool OptionList_Contains(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000AE2 RID: 2786
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_IndexOf")]
	public static extern int OptionList_IndexOf(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000AE3 RID: 2787
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_LastIndexOf")]
	public static extern int OptionList_LastIndexOf(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000AE4 RID: 2788
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionList_Remove")]
	public static extern bool OptionList_Remove(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000AE5 RID: 2789
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_OptionList")]
	public static extern void delete_OptionList(HandleRef jarg1);

	// Token: 0x06000AE6 RID: 2790
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_Clear")]
	public static extern void ModelObjectDataList_Clear(HandleRef jarg1);

	// Token: 0x06000AE7 RID: 2791
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_Add")]
	public static extern void ModelObjectDataList_Add(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000AE8 RID: 2792
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_size")]
	public static extern uint ModelObjectDataList_size(HandleRef jarg1);

	// Token: 0x06000AE9 RID: 2793
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_capacity")]
	public static extern uint ModelObjectDataList_capacity(HandleRef jarg1);

	// Token: 0x06000AEA RID: 2794
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_reserve")]
	public static extern void ModelObjectDataList_reserve(HandleRef jarg1, uint jarg2);

	// Token: 0x06000AEB RID: 2795
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ModelObjectDataList__SWIG_0")]
	public static extern IntPtr new_ModelObjectDataList__SWIG_0();

	// Token: 0x06000AEC RID: 2796
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ModelObjectDataList__SWIG_1")]
	public static extern IntPtr new_ModelObjectDataList__SWIG_1(HandleRef jarg1);

	// Token: 0x06000AED RID: 2797
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ModelObjectDataList__SWIG_2")]
	public static extern IntPtr new_ModelObjectDataList__SWIG_2(int jarg1);

	// Token: 0x06000AEE RID: 2798
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_getitemcopy")]
	public static extern IntPtr ModelObjectDataList_getitemcopy(HandleRef jarg1, int jarg2);

	// Token: 0x06000AEF RID: 2799
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_getitem")]
	public static extern IntPtr ModelObjectDataList_getitem(HandleRef jarg1, int jarg2);

	// Token: 0x06000AF0 RID: 2800
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_setitem")]
	public static extern void ModelObjectDataList_setitem(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000AF1 RID: 2801
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_AddRange")]
	public static extern void ModelObjectDataList_AddRange(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000AF2 RID: 2802
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_GetRange")]
	public static extern IntPtr ModelObjectDataList_GetRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000AF3 RID: 2803
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_Insert")]
	public static extern void ModelObjectDataList_Insert(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000AF4 RID: 2804
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_InsertRange")]
	public static extern void ModelObjectDataList_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000AF5 RID: 2805
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_RemoveAt")]
	public static extern void ModelObjectDataList_RemoveAt(HandleRef jarg1, int jarg2);

	// Token: 0x06000AF6 RID: 2806
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_RemoveRange")]
	public static extern void ModelObjectDataList_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000AF7 RID: 2807
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_Repeat")]
	public static extern IntPtr ModelObjectDataList_Repeat(HandleRef jarg1, int jarg2);

	// Token: 0x06000AF8 RID: 2808
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_Reverse__SWIG_0")]
	public static extern void ModelObjectDataList_Reverse__SWIG_0(HandleRef jarg1);

	// Token: 0x06000AF9 RID: 2809
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_Reverse__SWIG_1")]
	public static extern void ModelObjectDataList_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000AFA RID: 2810
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectDataList_SetRange")]
	public static extern void ModelObjectDataList_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000AFB RID: 2811
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ModelObjectDataList")]
	public static extern void delete_ModelObjectDataList(HandleRef jarg1);

	// Token: 0x06000AFC RID: 2812
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_Clear")]
	public static extern void StringList_Clear(HandleRef jarg1);

	// Token: 0x06000AFD RID: 2813
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_Add")]
	public static extern void StringList_Add(HandleRef jarg1, string jarg2);

	// Token: 0x06000AFE RID: 2814
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_size")]
	public static extern uint StringList_size(HandleRef jarg1);

	// Token: 0x06000AFF RID: 2815
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_capacity")]
	public static extern uint StringList_capacity(HandleRef jarg1);

	// Token: 0x06000B00 RID: 2816
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_reserve")]
	public static extern void StringList_reserve(HandleRef jarg1, uint jarg2);

	// Token: 0x06000B01 RID: 2817
	[DllImport("SDPCore", EntryPoint = "CSharp_new_StringList__SWIG_0")]
	public static extern IntPtr new_StringList__SWIG_0();

	// Token: 0x06000B02 RID: 2818
	[DllImport("SDPCore", EntryPoint = "CSharp_new_StringList__SWIG_1")]
	public static extern IntPtr new_StringList__SWIG_1(HandleRef jarg1);

	// Token: 0x06000B03 RID: 2819
	[DllImport("SDPCore", EntryPoint = "CSharp_new_StringList__SWIG_2")]
	public static extern IntPtr new_StringList__SWIG_2(int jarg1);

	// Token: 0x06000B04 RID: 2820
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_getitemcopy")]
	public static extern string StringList_getitemcopy(HandleRef jarg1, int jarg2);

	// Token: 0x06000B05 RID: 2821
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_getitem")]
	public static extern string StringList_getitem(HandleRef jarg1, int jarg2);

	// Token: 0x06000B06 RID: 2822
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_setitem")]
	public static extern void StringList_setitem(HandleRef jarg1, int jarg2, string jarg3);

	// Token: 0x06000B07 RID: 2823
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_AddRange")]
	public static extern void StringList_AddRange(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000B08 RID: 2824
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_GetRange")]
	public static extern IntPtr StringList_GetRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000B09 RID: 2825
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_Insert")]
	public static extern void StringList_Insert(HandleRef jarg1, int jarg2, string jarg3);

	// Token: 0x06000B0A RID: 2826
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_InsertRange")]
	public static extern void StringList_InsertRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000B0B RID: 2827
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_RemoveAt")]
	public static extern void StringList_RemoveAt(HandleRef jarg1, int jarg2);

	// Token: 0x06000B0C RID: 2828
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_RemoveRange")]
	public static extern void StringList_RemoveRange(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000B0D RID: 2829
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_Repeat")]
	public static extern IntPtr StringList_Repeat(string jarg1, int jarg2);

	// Token: 0x06000B0E RID: 2830
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_Reverse__SWIG_0")]
	public static extern void StringList_Reverse__SWIG_0(HandleRef jarg1);

	// Token: 0x06000B0F RID: 2831
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_Reverse__SWIG_1")]
	public static extern void StringList_Reverse__SWIG_1(HandleRef jarg1, int jarg2, int jarg3);

	// Token: 0x06000B10 RID: 2832
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_SetRange")]
	public static extern void StringList_SetRange(HandleRef jarg1, int jarg2, HandleRef jarg3);

	// Token: 0x06000B11 RID: 2833
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_Contains")]
	public static extern bool StringList_Contains(HandleRef jarg1, string jarg2);

	// Token: 0x06000B12 RID: 2834
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_IndexOf")]
	public static extern int StringList_IndexOf(HandleRef jarg1, string jarg2);

	// Token: 0x06000B13 RID: 2835
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_LastIndexOf")]
	public static extern int StringList_LastIndexOf(HandleRef jarg1, string jarg2);

	// Token: 0x06000B14 RID: 2836
	[DllImport("SDPCore", EntryPoint = "CSharp_StringList_Remove")]
	public static extern bool StringList_Remove(HandleRef jarg1, string jarg2);

	// Token: 0x06000B15 RID: 2837
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_StringList")]
	public static extern void delete_StringList(HandleRef jarg1);

	// Token: 0x06000B16 RID: 2838
	[DllImport("SDPCore", EntryPoint = "CSharp_INT_MAX_get")]
	public static extern int INT_MAX_get();

	// Token: 0x06000B17 RID: 2839
	[DllImport("SDPCore", EntryPoint = "CSharp_INT32_MAX_get")]
	public static extern int INT32_MAX_get();

	// Token: 0x06000B18 RID: 2840
	[DllImport("SDPCore", EntryPoint = "CSharp_UINT32_MAX_get")]
	public static extern int UINT32_MAX_get();

	// Token: 0x06000B19 RID: 2841
	[DllImport("SDPCore", EntryPoint = "CSharp_BufferKey_category_set")]
	public static extern void BufferKey_category_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000B1A RID: 2842
	[DllImport("SDPCore", EntryPoint = "CSharp_BufferKey_category_get")]
	public static extern uint BufferKey_category_get(HandleRef jarg1);

	// Token: 0x06000B1B RID: 2843
	[DllImport("SDPCore", EntryPoint = "CSharp_BufferKey_id_set")]
	public static extern void BufferKey_id_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000B1C RID: 2844
	[DllImport("SDPCore", EntryPoint = "CSharp_BufferKey_id_get")]
	public static extern uint BufferKey_id_get(HandleRef jarg1);

	// Token: 0x06000B1D RID: 2845
	[DllImport("SDPCore", EntryPoint = "CSharp_new_BufferKey")]
	public static extern IntPtr new_BufferKey(uint jarg1, uint jarg2);

	// Token: 0x06000B1E RID: 2846
	[DllImport("SDPCore", EntryPoint = "CSharp_BufferKey_EqualTo")]
	public static extern bool BufferKey_EqualTo(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000B1F RID: 2847
	[DllImport("SDPCore", EntryPoint = "CSharp_BufferKey_LessThan")]
	public static extern bool BufferKey_LessThan(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000B20 RID: 2848
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_BufferKey")]
	public static extern void delete_BufferKey(HandleRef jarg1);

	// Token: 0x06000B21 RID: 2849
	[DllImport("SDPCore", EntryPoint = "CSharp_new_BinaryDataPair__SWIG_0")]
	public static extern IntPtr new_BinaryDataPair__SWIG_0();

	// Token: 0x06000B22 RID: 2850
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_BinaryDataPair")]
	public static extern void delete_BinaryDataPair(HandleRef jarg1);

	// Token: 0x06000B23 RID: 2851
	[DllImport("SDPCore", EntryPoint = "CSharp_new_BinaryDataPair__SWIG_1")]
	public static extern IntPtr new_BinaryDataPair__SWIG_1(HandleRef jarg1);

	// Token: 0x06000B24 RID: 2852
	[DllImport("SDPCore", EntryPoint = "CSharp_BinaryDataPair_IsValid")]
	public static extern bool BinaryDataPair_IsValid(HandleRef jarg1);

	// Token: 0x06000B25 RID: 2853
	[DllImport("SDPCore", EntryPoint = "CSharp_BinaryDataPair_Equal")]
	public static extern IntPtr BinaryDataPair_Equal(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000B26 RID: 2854
	[DllImport("SDPCore", EntryPoint = "CSharp_BinaryDataPair_size_set")]
	public static extern void BinaryDataPair_size_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000B27 RID: 2855
	[DllImport("SDPCore", EntryPoint = "CSharp_BinaryDataPair_size_get")]
	public static extern uint BinaryDataPair_size_get(HandleRef jarg1);

	// Token: 0x06000B28 RID: 2856
	[DllImport("SDPCore", EntryPoint = "CSharp_BinaryDataPair_data_set")]
	public static extern void BinaryDataPair_data_set(HandleRef jarg1, IntPtr jarg2);

	// Token: 0x06000B29 RID: 2857
	[DllImport("SDPCore", EntryPoint = "CSharp_BinaryDataPair_data_get")]
	public static extern IntPtr BinaryDataPair_data_get(HandleRef jarg1);

	// Token: 0x06000B2A RID: 2858
	[DllImport("SDPCore", EntryPoint = "CSharp_BinaryDataPair_FinalizeCallback_set")]
	public static extern void BinaryDataPair_FinalizeCallback_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000B2B RID: 2859
	[DllImport("SDPCore", EntryPoint = "CSharp_BinaryDataPair_FinalizeCallback_get")]
	public static extern IntPtr BinaryDataPair_FinalizeCallback_get(HandleRef jarg1);

	// Token: 0x06000B2C RID: 2860
	[DllImport("SDPCore", EntryPoint = "CSharp_CLIENT_IP_FILE_NAME_get")]
	public static extern string CLIENT_IP_FILE_NAME_get();

	// Token: 0x06000B2D RID: 2861
	[DllImport("SDPCore", EntryPoint = "CSharp_PORT_NUMBER_get")]
	public static extern uint PORT_NUMBER_get();

	// Token: 0x06000B2E RID: 2862
	[DllImport("SDPCore", EntryPoint = "CSharp_LOCALHOST_IP_get")]
	public static extern string LOCALHOST_IP_get();

	// Token: 0x06000B2F RID: 2863
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_MAX_STRING_LENGTH_get")]
	public static extern uint SDP_MAX_STRING_LENGTH_get();

	// Token: 0x06000B30 RID: 2864
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_ICON_WIDTH_get")]
	public static extern uint SDP_ICON_WIDTH_get();

	// Token: 0x06000B31 RID: 2865
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_ICON_HEIGHT_get")]
	public static extern uint SDP_ICON_HEIGHT_get();

	// Token: 0x06000B32 RID: 2866
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_ICON_SIZE_get")]
	public static extern uint SDP_ICON_SIZE_get();

	// Token: 0x06000B33 RID: 2867
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_DEFAULT_CAPTURE_get")]
	public static extern uint SDP_DEFAULT_CAPTURE_get();

	// Token: 0x06000B34 RID: 2868
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_CAPTURE_UNKNOWN_get")]
	public static extern uint SDP_CAPTURE_UNKNOWN_get();

	// Token: 0x06000B35 RID: 2869
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_ANY_PID_get")]
	public static extern uint SDP_ANY_PID_get();

	// Token: 0x06000B36 RID: 2870
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_GLOBAL_PID_get")]
	public static extern uint SDP_GLOBAL_PID_get();

	// Token: 0x06000B37 RID: 2871
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_UNUSED_get")]
	public static extern uint SDP_UNUSED_get();

	// Token: 0x06000B38 RID: 2872
	[DllImport("SDPCore", EntryPoint = "CSharp_LRZStateMetricName_get")]
	public static extern string LRZStateMetricName_get();

	// Token: 0x06000B39 RID: 2873
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_USER_DATA_CAPTURE_SUPPORT_get")]
	public static extern uint SDP_USER_DATA_CAPTURE_SUPPORT_get();

	// Token: 0x06000B3A RID: 2874
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_CAPTURETYPE_NONE_get")]
	public static extern uint SDP_CAPTURETYPE_NONE_get();

	// Token: 0x06000B3B RID: 2875
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_CAPTURETYPE_REALTIME_get")]
	public static extern uint SDP_CAPTURETYPE_REALTIME_get();

	// Token: 0x06000B3C RID: 2876
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_CAPTURETYPE_TRACE_get")]
	public static extern uint SDP_CAPTURETYPE_TRACE_get();

	// Token: 0x06000B3D RID: 2877
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_CAPTURETYPE_SNAPSHOT_get")]
	public static extern uint SDP_CAPTURETYPE_SNAPSHOT_get();

	// Token: 0x06000B3E RID: 2878
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_CAPTURETYPE_SAMPLE_get")]
	public static extern uint SDP_CAPTURETYPE_SAMPLE_get();

	// Token: 0x06000B3F RID: 2879
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_CAPTURETYPE_MAX_BIT_get")]
	public static extern uint SDP_CAPTURETYPE_MAX_BIT_get();

	// Token: 0x06000B40 RID: 2880
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_CAPTURETYPE_ALL_get")]
	public static extern uint SDP_CAPTURETYPE_ALL_get();

	// Token: 0x06000B41 RID: 2881
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_DATAPROVIDER_ALL_get")]
	public static extern uint SDP_DATAPROVIDER_ALL_get();

	// Token: 0x06000B42 RID: 2882
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_OPTION_ATTR_NONE_get")]
	public static extern uint SDP_OPTION_ATTR_NONE_get();

	// Token: 0x06000B43 RID: 2883
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_OPTION_ATTR_HIDDEN_get")]
	public static extern uint SDP_OPTION_ATTR_HIDDEN_get();

	// Token: 0x06000B44 RID: 2884
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_OPTION_ATTR_READONLY_get")]
	public static extern uint SDP_OPTION_ATTR_READONLY_get();

	// Token: 0x06000B45 RID: 2885
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_OPTION_ATTR_PROC_INFO_get")]
	public static extern uint SDP_OPTION_ATTR_PROC_INFO_get();

	// Token: 0x06000B46 RID: 2886
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_OPTION_ATTR_CONTEXT_STATE_get")]
	public static extern uint SDP_OPTION_ATTR_CONTEXT_STATE_get();

	// Token: 0x06000B47 RID: 2887
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_OPTION_ATTR_LAUNCH_APP_get")]
	public static extern uint SDP_OPTION_ATTR_LAUNCH_APP_get();

	// Token: 0x06000B48 RID: 2888
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_UNKNOWN_get")]
	public static extern uint BUFFER_TYPE_UNKNOWN_get();

	// Token: 0x06000B49 RID: 2889
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_PERFETTO_PROTOBUF_get")]
	public static extern uint BUFFER_TYPE_PERFETTO_PROTOBUF_get();

	// Token: 0x06000B4A RID: 2890
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_SYSTRACE_DATA_get")]
	public static extern uint BUFFER_TYPE_SYSTRACE_DATA_get();

	// Token: 0x06000B4B RID: 2891
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_DATA_get")]
	public static extern uint BUFFER_TYPE_GLES_DATA_get();

	// Token: 0x06000B4C RID: 2892
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_STRIPPED_DCAP_get")]
	public static extern uint BUFFER_TYPE_GLES_STRIPPED_DCAP_get();

	// Token: 0x06000B4D RID: 2893
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_FULL_DCAP_get")]
	public static extern uint BUFFER_TYPE_GLES_FULL_DCAP_get();

	// Token: 0x06000B4E RID: 2894
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_SHADER_STAT_get")]
	public static extern uint BUFFER_TYPE_GLES_SHADER_STAT_get();

	// Token: 0x06000B4F RID: 2895
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_BUFFER_DATA_get")]
	public static extern uint BUFFER_TYPE_GLES_BUFFER_DATA_get();

	// Token: 0x06000B50 RID: 2896
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_CAPTURE_SCREENSHOT_get")]
	public static extern uint BUFFER_TYPE_GLES_CAPTURE_SCREENSHOT_get();

	// Token: 0x06000B51 RID: 2897
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_CL_KA_KERNEL_STAT_get")]
	public static extern uint BUFFER_TYPE_CL_KA_KERNEL_STAT_get();

	// Token: 0x06000B52 RID: 2898
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_QGL_DATA_get")]
	public static extern uint BUFFER_TYPE_QGL_DATA_get();

	// Token: 0x06000B53 RID: 2899
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_QGL_STRIPPED_DCAP_get")]
	public static extern uint BUFFER_TYPE_QGL_STRIPPED_DCAP_get();

	// Token: 0x06000B54 RID: 2900
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_QGL_FULL_DCAP_get")]
	public static extern uint BUFFER_TYPE_QGL_FULL_DCAP_get();

	// Token: 0x06000B55 RID: 2901
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_QGL_SHADER_STAT_get")]
	public static extern uint BUFFER_TYPE_QGL_SHADER_STAT_get();

	// Token: 0x06000B56 RID: 2902
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_QGL_BUFFER_DATA_get")]
	public static extern uint BUFFER_TYPE_QGL_BUFFER_DATA_get();

	// Token: 0x06000B57 RID: 2903
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_QGL_CAPTURE_SCREENSHOT_get")]
	public static extern uint BUFFER_TYPE_QGL_CAPTURE_SCREENSHOT_get();

	// Token: 0x06000B58 RID: 2904
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_QGL_TRACE_DATA_get")]
	public static extern uint BUFFER_TYPE_QGL_TRACE_DATA_get();

	// Token: 0x06000B59 RID: 2905
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_ADSP_DATA_get")]
	public static extern uint BUFFER_TYPE_ADSP_DATA_get();

	// Token: 0x06000B5A RID: 2906
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_ADSP_METRIC_EVENT_MAP_get")]
	public static extern uint BUFFER_TYPE_ADSP_METRIC_EVENT_MAP_get();

	// Token: 0x06000B5B RID: 2907
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_SDSP_DATA_get")]
	public static extern uint BUFFER_TYPE_SDSP_DATA_get();

	// Token: 0x06000B5C RID: 2908
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_SDSP_METRIC_EVENT_MAP_get")]
	public static extern uint BUFFER_TYPE_SDSP_METRIC_EVENT_MAP_get();

	// Token: 0x06000B5D RID: 2909
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_CDSP_DATA_get")]
	public static extern uint BUFFER_TYPE_CDSP_DATA_get();

	// Token: 0x06000B5E RID: 2910
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_CDSP_METRIC_EVENT_MAP_get")]
	public static extern uint BUFFER_TYPE_CDSP_METRIC_EVENT_MAP_get();

	// Token: 0x06000B5F RID: 2911
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_VULKAN_REPLAY_DATA_get")]
	public static extern uint BUFFER_TYPE_VULKAN_REPLAY_DATA_get();

	// Token: 0x06000B60 RID: 2912
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA_get")]
	public static extern uint BUFFER_TYPE_VULKAN_REPLAY_METRICS_DATA_get();

	// Token: 0x06000B61 RID: 2913
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_VULKAN_REPLAY_SCOPE_SHADER_PROFILES_DATA_get")]
	public static extern uint BUFFER_TYPE_VULKAN_REPLAY_SCOPE_SHADER_PROFILES_DATA_get();

	// Token: 0x06000B62 RID: 2914
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_CL_TRACE_DATA_get")]
	public static extern uint BUFFER_TYPE_CL_TRACE_DATA_get();

	// Token: 0x06000B63 RID: 2915
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_CL_ROOFLINE_DATA_get")]
	public static extern uint BUFFER_TYPE_CL_ROOFLINE_DATA_get();

	// Token: 0x06000B64 RID: 2916
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_NPU_DATA_get")]
	public static extern uint BUFFER_TYPE_NPU_DATA_get();

	// Token: 0x06000B65 RID: 2917
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_NPU_METRIC_EVENT_MAP_get")]
	public static extern uint BUFFER_TYPE_NPU_METRIC_EVENT_MAP_get();

	// Token: 0x06000B66 RID: 2918
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_QGL_CMDBUF_DATA_get")]
	public static extern uint BUFFER_TYPE_QGL_CMDBUF_DATA_get();

	// Token: 0x06000B67 RID: 2919
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_QGL_DATA_BOTH_get")]
	public static extern uint BUFFER_TYPE_QGL_DATA_BOTH_get();

	// Token: 0x06000B68 RID: 2920
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GGPM_TRACE_DATA_get")]
	public static extern uint BUFFER_TYPE_GGPM_TRACE_DATA_get();

	// Token: 0x06000B69 RID: 2921
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_ERROR_MSG_get")]
	public static extern uint BUFFER_TYPE_ERROR_MSG_get();

	// Token: 0x06000B6A RID: 2922
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_BIT_get")]
	public static extern uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_BIT_get();

	// Token: 0x06000B6B RID: 2923
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_ADRENOINSIGHTBA_get")]
	public static extern uint BUFFER_TYPE_ADRENOINSIGHTBA_get();

	// Token: 0x06000B6C RID: 2924
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_ADRENOINSIGHTDCAP_get")]
	public static extern uint BUFFER_TYPE_ADRENOINSIGHTDCAP_get();

	// Token: 0x06000B6D RID: 2925
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_DATA_get")]
	public static extern uint BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_DATA_get();

	// Token: 0x06000B6E RID: 2926
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_FILE_DATA_get")]
	public static extern uint BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_FILE_DATA_get();

	// Token: 0x06000B6F RID: 2927
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_STRIPPED_DATA_get")]
	public static extern uint BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_STRIPPED_DATA_get();

	// Token: 0x06000B70 RID: 2928
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_STRIPPED_FILE_DATA_get")]
	public static extern uint BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_STRIPPED_FILE_DATA_get();

	// Token: 0x06000B71 RID: 2929
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_EXPORT_GLTF_get")]
	public static extern uint BUFFER_TYPE_VULKAN_GFXRECONSTRUCT_EXPORT_GLTF_get();

	// Token: 0x06000B72 RID: 2930
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA_get")]
	public static extern uint BUFFER_TYPE_VULKAN_SNAPSHOT_PROCESSED_API_DATA_get();

	// Token: 0x06000B73 RID: 2931
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_VULKAN_PUSH_CONSTANT_REFLECTION_get")]
	public static extern uint BUFFER_TYPE_VULKAN_PUSH_CONSTANT_REFLECTION_get();

	// Token: 0x06000B74 RID: 2932
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_VULKAN_SPIRV_CROSS_SHADER_SOURCE_DATA_get")]
	public static extern uint BUFFER_TYPE_VULKAN_SPIRV_CROSS_SHADER_SOURCE_DATA_get();

	// Token: 0x06000B75 RID: 2933
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_VULKAN_UNIFORM_BUFFER_REFLECTION_get")]
	public static extern uint BUFFER_TYPE_VULKAN_UNIFORM_BUFFER_REFLECTION_get();

	// Token: 0x06000B76 RID: 2934
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_VULKAN_REPLAY_HANDLE_MAPPING_get")]
	public static extern uint BUFFER_TYPE_VULKAN_REPLAY_HANDLE_MAPPING_get();

	// Token: 0x06000B77 RID: 2935
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_MARKER_DATA_get")]
	public static extern uint BUFFER_TYPE_MARKER_DATA_get();

	// Token: 0x06000B78 RID: 2936
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_CPU_PERF_GLOBAL_DATA_get")]
	public static extern uint BUFFER_TYPE_CPU_PERF_GLOBAL_DATA_get();

	// Token: 0x06000B79 RID: 2937
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_CPU_PERF_PROCESS_DATA_get")]
	public static extern uint BUFFER_TYPE_CPU_PERF_PROCESS_DATA_get();

	// Token: 0x06000B7A RID: 2938
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_SHADER_STAT_CSV_get")]
	public static extern uint BUFFER_TYPE_GLES_SHADER_STAT_CSV_get();

	// Token: 0x06000B7B RID: 2939
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_SHADER_DISASM_get")]
	public static extern uint BUFFER_TYPE_GLES_SHADER_DISASM_get();

	// Token: 0x06000B7C RID: 2940
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_VULKAN_SNAPSHOT_SHADER_DATA_get")]
	public static extern uint BUFFER_TYPE_VULKAN_SNAPSHOT_SHADER_DATA_get();

	// Token: 0x06000B7D RID: 2941
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_VULKAN_TRACE_SHADER_DATA_get")]
	public static extern uint BUFFER_TYPE_VULKAN_TRACE_SHADER_DATA_get();

	// Token: 0x06000B7E RID: 2942
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_DX11_TRACE_DATA_get")]
	public static extern uint BUFFER_TYPE_DX11_TRACE_DATA_get();

	// Token: 0x06000B7F RID: 2943
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_DX12_TRACE_DATA_get")]
	public static extern uint BUFFER_TYPE_DX12_TRACE_DATA_get();

	// Token: 0x06000B80 RID: 2944
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_DX12_GFXRECONSTRUCT_DATA_get")]
	public static extern uint BUFFER_TYPE_DX12_GFXRECONSTRUCT_DATA_get();

	// Token: 0x06000B81 RID: 2945
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_DX12_GFXRECONSTRUCT_FILE_DATA_get")]
	public static extern uint BUFFER_TYPE_DX12_GFXRECONSTRUCT_FILE_DATA_get();

	// Token: 0x06000B82 RID: 2946
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_DX12_GFXRECONSTRUCT_OPTIMIZE_FILE_DATA_get")]
	public static extern uint BUFFER_TYPE_DX12_GFXRECONSTRUCT_OPTIMIZE_FILE_DATA_get();

	// Token: 0x06000B83 RID: 2947
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_DX12_GFXRECONSTRUCT_STRIPPED_DATA_get")]
	public static extern uint BUFFER_TYPE_DX12_GFXRECONSTRUCT_STRIPPED_DATA_get();

	// Token: 0x06000B84 RID: 2948
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_DX12_GFXRECONSTRUCT_STRIPPED_FILE_DATA_get")]
	public static extern uint BUFFER_TYPE_DX12_GFXRECONSTRUCT_STRIPPED_FILE_DATA_get();

	// Token: 0x06000B85 RID: 2949
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_WINCPU_TRACE_DATA_get")]
	public static extern uint BUFFER_TYPE_WINCPU_TRACE_DATA_get();

	// Token: 0x06000B86 RID: 2950
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_DX12_SNAPSHOT_ATTACHMENTS_DATA_get")]
	public static extern uint BUFFER_TYPE_DX12_SNAPSHOT_ATTACHMENTS_DATA_get();

	// Token: 0x06000B87 RID: 2951
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_DX12_SNAPSHOT_METRICS_DATA_get")]
	public static extern uint BUFFER_TYPE_DX12_SNAPSHOT_METRICS_DATA_get();

	// Token: 0x06000B88 RID: 2952
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_DX12_SNAPSHOT_SHADER_DATA_get")]
	public static extern uint BUFFER_TYPE_DX12_SNAPSHOT_SHADER_DATA_get();

	// Token: 0x06000B89 RID: 2953
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_DX12_SNAPSHOT_PROCESSED_API_DATA_get")]
	public static extern uint BUFFER_TYPE_DX12_SNAPSHOT_PROCESSED_API_DATA_get();

	// Token: 0x06000B8A RID: 2954
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_DX12_SNAPSHOT_PROCESSED_METRICS_DATA_get")]
	public static extern uint BUFFER_TYPE_DX12_SNAPSHOT_PROCESSED_METRICS_DATA_get();

	// Token: 0x06000B8B RID: 2955
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_SERVICE_FILE_get")]
	public static extern uint BUFFER_TYPE_SERVICE_FILE_get();

	// Token: 0x06000B8C RID: 2956
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_DEVICE_FILE_get")]
	public static extern uint BUFFER_TYPE_DEVICE_FILE_get();

	// Token: 0x06000B8D RID: 2957
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_READ_API_FROM_GFXR_get")]
	public static extern uint BUFFER_TYPE_READ_API_FROM_GFXR_get();

	// Token: 0x06000B8E RID: 2958
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_READ_VULKAN_FROM_GFXR_get")]
	public static extern uint BUFFER_TYPE_READ_VULKAN_FROM_GFXR_get();

	// Token: 0x06000B8F RID: 2959
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_READ_DX12_FROM_GFXR_get")]
	public static extern uint BUFFER_TYPE_READ_DX12_FROM_GFXR_get();

	// Token: 0x06000B90 RID: 2960
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_SNAPSHOT_FROM_GFXR_get")]
	public static extern uint BUFFER_TYPE_SNAPSHOT_FROM_GFXR_get();

	// Token: 0x06000B91 RID: 2961
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_EXPORT_GFXRECONSTRUCT_DATA_get")]
	public static extern uint BUFFER_TYPE_EXPORT_GFXRECONSTRUCT_DATA_get();

	// Token: 0x06000B92 RID: 2962
	[DllImport("SDPCore", EntryPoint = "CSharp_IsBufferCategoryApiTraceData")]
	public static extern bool IsBufferCategoryApiTraceData(uint jarg1);

	// Token: 0x06000B93 RID: 2963
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_PLAYBACK_FULLSIZE_BIT_get")]
	public static extern uint BUFFER_TYPE_GLES_PLAYBACK_FULLSIZE_BIT_get();

	// Token: 0x06000B94 RID: 2964
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_COLOR_THUMB_get")]
	public static extern uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_COLOR_THUMB_get();

	// Token: 0x06000B95 RID: 2965
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_HIGHLIGHT_THUMB_get")]
	public static extern uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_HIGHLIGHT_THUMB_get();

	// Token: 0x06000B96 RID: 2966
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_DEPTH_THUMB_get")]
	public static extern uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_DEPTH_THUMB_get();

	// Token: 0x06000B97 RID: 2967
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_STENCIL_THUMB_get")]
	public static extern uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_STENCIL_THUMB_get();

	// Token: 0x06000B98 RID: 2968
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_COLOR_FULL_get")]
	public static extern uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_COLOR_FULL_get();

	// Token: 0x06000B99 RID: 2969
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_HIGHLIGHT_FULL_get")]
	public static extern uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_HIGHLIGHT_FULL_get();

	// Token: 0x06000B9A RID: 2970
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_DEPTH_FULL_get")]
	public static extern uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_DEPTH_FULL_get();

	// Token: 0x06000B9B RID: 2971
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_STENCIL_FULL_get")]
	public static extern uint BUFFER_TYPE_GLES_PLAYBACK_SCREENSHOT_STENCIL_FULL_get();

	// Token: 0x06000B9C RID: 2972
	[DllImport("SDPCore", EntryPoint = "CSharp_BUFFER_ID_GFXRECONSTRUCT_ERROR_get")]
	public static extern uint BUFFER_ID_GFXRECONSTRUCT_ERROR_get();

	// Token: 0x06000B9D RID: 2973
	[DllImport("SDPCore", EntryPoint = "CSharp_PLUGINS_DIR_get")]
	public static extern string PLUGINS_DIR_get();

	// Token: 0x06000B9E RID: 2974
	[DllImport("SDPCore", EntryPoint = "CSharp_DATA_PLUGIN_DIR_get")]
	public static extern string DATA_PLUGIN_DIR_get();

	// Token: 0x06000B9F RID: 2975
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_MONITOR_REQ_HASH_FILE_get")]
	public static extern uint SDPNET_MONITOR_REQ_HASH_FILE_get();

	// Token: 0x06000BA0 RID: 2976
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_MONITOR_REP_HASH_FILE_get")]
	public static extern uint SDPNET_MONITOR_REP_HASH_FILE_get();

	// Token: 0x06000BA1 RID: 2977
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_MONITOR_START_SERVICE_get")]
	public static extern uint SDPNET_MONITOR_START_SERVICE_get();

	// Token: 0x06000BA2 RID: 2978
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_MONITOR_STOP_SERVICE_get")]
	public static extern uint SDPNET_MONITOR_STOP_SERVICE_get();

	// Token: 0x06000BA3 RID: 2979
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_MONITOR_UPDATE_SERVICE_get")]
	public static extern uint SDPNET_MONITOR_UPDATE_SERVICE_get();

	// Token: 0x06000BA4 RID: 2980
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_MONITOR_REQ_UPDATE_SERVICE_COMPLETE_get")]
	public static extern uint SDPNET_MONITOR_REQ_UPDATE_SERVICE_COMPLETE_get();

	// Token: 0x06000BA5 RID: 2981
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_MONITOR_REP_UPDATE_SERVICE_COMPLETE_get")]
	public static extern uint SDPNET_MONITOR_REP_UPDATE_SERVICE_COMPLETE_get();

	// Token: 0x06000BA6 RID: 2982
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_MONITOR_REGISTER_CLIENT_get")]
	public static extern uint SDPNET_MONITOR_REGISTER_CLIENT_get();

	// Token: 0x06000BA7 RID: 2983
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_MONITOR_UNINSTALL_SERVICE_get")]
	public static extern uint SDPNET_MONITOR_UNINSTALL_SERVICE_get();

	// Token: 0x06000BA8 RID: 2984
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_MONITOR_REQ_CHECK_VERSION_get")]
	public static extern uint SDPNET_MONITOR_REQ_CHECK_VERSION_get();

	// Token: 0x06000BA9 RID: 2985
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_MONITOR_REP_CHECK_VERSION_get")]
	public static extern uint SDPNET_MONITOR_REP_CHECK_VERSION_get();

	// Token: 0x06000BAA RID: 2986
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_MONITOR_VERSION_get")]
	public static extern uint SDP_MONITOR_VERSION_get();

	// Token: 0x06000BAB RID: 2987
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_VERSION_MAJOR_get")]
	public static extern int SDP_VERSION_MAJOR_get();

	// Token: 0x06000BAC RID: 2988
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_VERSION_MINOR_get")]
	public static extern int SDP_VERSION_MINOR_get();

	// Token: 0x06000BAD RID: 2989
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_VERSION_SUBMINOR_get")]
	public static extern int SDP_VERSION_SUBMINOR_get();

	// Token: 0x06000BAE RID: 2990
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_VERSION_MAJOR_MINOR_get")]
	public static extern string SDP_VERSION_MAJOR_MINOR_get();

	// Token: 0x06000BAF RID: 2991
	[DllImport("SDPCore", EntryPoint = "CSharp_new_SDPVersion")]
	public static extern IntPtr new_SDPVersion();

	// Token: 0x06000BB0 RID: 2992
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPVersion_GetFullVersionString")]
	public static extern string SDPVersion_GetFullVersionString(HandleRef jarg1);

	// Token: 0x06000BB1 RID: 2993
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPVersion_GetVersion")]
	public static extern long SDPVersion_GetVersion(HandleRef jarg1);

	// Token: 0x06000BB2 RID: 2994
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPVersion_GetVersionString")]
	public static extern string SDPVersion_GetVersionString(HandleRef jarg1);

	// Token: 0x06000BB3 RID: 2995
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPVersion_GetBuildDate")]
	public static extern string SDPVersion_GetBuildDate(HandleRef jarg1);

	// Token: 0x06000BB4 RID: 2996
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_SDPVersion")]
	public static extern void delete_SDPVersion(HandleRef jarg1);

	// Token: 0x06000BB5 RID: 2997
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_NetCommandDelegate")]
	public static extern void delete_NetCommandDelegate(HandleRef jarg1);

	// Token: 0x06000BB6 RID: 2998
	[DllImport("SDPCore", EntryPoint = "CSharp_NetCommandDelegate_OnConnected")]
	public static extern void NetCommandDelegate_OnConnected(HandleRef jarg1);

	// Token: 0x06000BB7 RID: 2999
	[DllImport("SDPCore", EntryPoint = "CSharp_NetCommandDelegate_OnDisconnected")]
	public static extern void NetCommandDelegate_OnDisconnected(HandleRef jarg1);

	// Token: 0x06000BB8 RID: 3000
	[DllImport("SDPCore", EntryPoint = "CSharp_NetCommandDelegate_OnClientConnected")]
	public static extern void NetCommandDelegate_OnClientConnected(HandleRef jarg1, uint jarg2);

	// Token: 0x06000BB9 RID: 3001
	[DllImport("SDPCore", EntryPoint = "CSharp_NetCommandDelegate_OnClientDisconnected")]
	public static extern void NetCommandDelegate_OnClientDisconnected(HandleRef jarg1, uint jarg2);

	// Token: 0x06000BBA RID: 3002
	[DllImport("SDPCore", EntryPoint = "CSharp_NetCommandDelegate_OnMessageReceived")]
	public static extern void NetCommandDelegate_OnMessageReceived(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000BBB RID: 3003
	[DllImport("SDPCore", EntryPoint = "CSharp_new_NetCommandDelegate")]
	public static extern IntPtr new_NetCommandDelegate();

	// Token: 0x06000BBC RID: 3004
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_deviceName_set")]
	public static extern void DeviceAttributes_deviceName_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BBD RID: 3005
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_deviceName_get")]
	public static extern string DeviceAttributes_deviceName_get(HandleRef jarg1);

	// Token: 0x06000BBE RID: 3006
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_productName_set")]
	public static extern void DeviceAttributes_productName_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BBF RID: 3007
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_productName_get")]
	public static extern string DeviceAttributes_productName_get(HandleRef jarg1);

	// Token: 0x06000BC0 RID: 3008
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_productModel_set")]
	public static extern void DeviceAttributes_productModel_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BC1 RID: 3009
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_productModel_get")]
	public static extern string DeviceAttributes_productModel_get(HandleRef jarg1);

	// Token: 0x06000BC2 RID: 3010
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_productManufacturer_set")]
	public static extern void DeviceAttributes_productManufacturer_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BC3 RID: 3011
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_productManufacturer_get")]
	public static extern string DeviceAttributes_productManufacturer_get(HandleRef jarg1);

	// Token: 0x06000BC4 RID: 3012
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_productBrand_set")]
	public static extern void DeviceAttributes_productBrand_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BC5 RID: 3013
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_productBrand_get")]
	public static extern string DeviceAttributes_productBrand_get(HandleRef jarg1);

	// Token: 0x06000BC6 RID: 3014
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_productLocaleRegion_set")]
	public static extern void DeviceAttributes_productLocaleRegion_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BC7 RID: 3015
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_productLocaleRegion_get")]
	public static extern string DeviceAttributes_productLocaleRegion_get(HandleRef jarg1);

	// Token: 0x06000BC8 RID: 3016
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_productLocaleLanguage_set")]
	public static extern void DeviceAttributes_productLocaleLanguage_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BC9 RID: 3017
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_productLocaleLanguage_get")]
	public static extern string DeviceAttributes_productLocaleLanguage_get(HandleRef jarg1);

	// Token: 0x06000BCA RID: 3018
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_buildProduct_set")]
	public static extern void DeviceAttributes_buildProduct_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BCB RID: 3019
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_buildProduct_get")]
	public static extern string DeviceAttributes_buildProduct_get(HandleRef jarg1);

	// Token: 0x06000BCC RID: 3020
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_buildVersionRelease_set")]
	public static extern void DeviceAttributes_buildVersionRelease_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BCD RID: 3021
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_buildVersionRelease_get")]
	public static extern string DeviceAttributes_buildVersionRelease_get(HandleRef jarg1);

	// Token: 0x06000BCE RID: 3022
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_buildVersionSDK_set")]
	public static extern void DeviceAttributes_buildVersionSDK_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BCF RID: 3023
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_buildVersionSDK_get")]
	public static extern string DeviceAttributes_buildVersionSDK_get(HandleRef jarg1);

	// Token: 0x06000BD0 RID: 3024
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_buildDate_set")]
	public static extern void DeviceAttributes_buildDate_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BD1 RID: 3025
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_buildDate_get")]
	public static extern string DeviceAttributes_buildDate_get(HandleRef jarg1);

	// Token: 0x06000BD2 RID: 3026
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_buildDescription_set")]
	public static extern void DeviceAttributes_buildDescription_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BD3 RID: 3027
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_buildDescription_get")]
	public static extern string DeviceAttributes_buildDescription_get(HandleRef jarg1);

	// Token: 0x06000BD4 RID: 3028
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_boardPlatform_set")]
	public static extern void DeviceAttributes_boardPlatform_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BD5 RID: 3029
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_boardPlatform_get")]
	public static extern string DeviceAttributes_boardPlatform_get(HandleRef jarg1);

	// Token: 0x06000BD6 RID: 3030
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_osType_set")]
	public static extern void DeviceAttributes_osType_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BD7 RID: 3031
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_osType_get")]
	public static extern string DeviceAttributes_osType_get(HandleRef jarg1);

	// Token: 0x06000BD8 RID: 3032
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_abiList_set")]
	public static extern void DeviceAttributes_abiList_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BD9 RID: 3033
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_abiList_get")]
	public static extern string DeviceAttributes_abiList_get(HandleRef jarg1);

	// Token: 0x06000BDA RID: 3034
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_buildDateUTC_set")]
	public static extern void DeviceAttributes_buildDateUTC_set(HandleRef jarg1, long jarg2);

	// Token: 0x06000BDB RID: 3035
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_buildDateUTC_get")]
	public static extern long DeviceAttributes_buildDateUTC_get(HandleRef jarg1);

	// Token: 0x06000BDC RID: 3036
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_updatedGraphicsDriver_set")]
	public static extern void DeviceAttributes_updatedGraphicsDriver_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BDD RID: 3037
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_updatedGraphicsDriver_get")]
	public static extern string DeviceAttributes_updatedGraphicsDriver_get(HandleRef jarg1);

	// Token: 0x06000BDE RID: 3038
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_gameDriverPrereleaseOptInApps_set")]
	public static extern void DeviceAttributes_gameDriverPrereleaseOptInApps_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BDF RID: 3039
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_gameDriverPrereleaseOptInApps_get")]
	public static extern string DeviceAttributes_gameDriverPrereleaseOptInApps_get(HandleRef jarg1);

	// Token: 0x06000BE0 RID: 3040
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_autoDetected_set")]
	public static extern void DeviceAttributes_autoDetected_set(HandleRef jarg1, bool jarg2);

	// Token: 0x06000BE1 RID: 3041
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_autoDetected_get")]
	public static extern bool DeviceAttributes_autoDetected_get(HandleRef jarg1);

	// Token: 0x06000BE2 RID: 3042
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_GetProductModel")]
	public static extern string DeviceAttributes_GetProductModel(HandleRef jarg1);

	// Token: 0x06000BE3 RID: 3043
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_GetBuildVersionRelease")]
	public static extern string DeviceAttributes_GetBuildVersionRelease(HandleRef jarg1);

	// Token: 0x06000BE4 RID: 3044
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceAttributes_GetBuildDateUTC")]
	public static extern long DeviceAttributes_GetBuildDateUTC(HandleRef jarg1);

	// Token: 0x06000BE5 RID: 3045
	[DllImport("SDPCore", EntryPoint = "CSharp_new_DeviceAttributes")]
	public static extern IntPtr new_DeviceAttributes();

	// Token: 0x06000BE6 RID: 3046
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_DeviceAttributes")]
	public static extern void delete_DeviceAttributes(HandleRef jarg1);

	// Token: 0x06000BE7 RID: 3047
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceProperties_connectionType_set")]
	public static extern void DeviceProperties_connectionType_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000BE8 RID: 3048
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceProperties_connectionType_get")]
	public static extern uint DeviceProperties_connectionType_get(HandleRef jarg1);

	// Token: 0x06000BE9 RID: 3049
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceProperties_name_set")]
	public static extern void DeviceProperties_name_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BEA RID: 3050
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceProperties_name_get")]
	public static extern string DeviceProperties_name_get(HandleRef jarg1);

	// Token: 0x06000BEB RID: 3051
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceProperties_ipAddress_set")]
	public static extern void DeviceProperties_ipAddress_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BEC RID: 3052
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceProperties_ipAddress_get")]
	public static extern string DeviceProperties_ipAddress_get(HandleRef jarg1);

	// Token: 0x06000BED RID: 3053
	[DllImport("SDPCore", EntryPoint = "CSharp_new_DeviceProperties")]
	public static extern IntPtr new_DeviceProperties();

	// Token: 0x06000BEE RID: 3054
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_DeviceProperties")]
	public static extern void delete_DeviceProperties(HandleRef jarg1);

	// Token: 0x06000BEF RID: 3055
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartSettings_executablePath_set")]
	public static extern void AppStartSettings_executablePath_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BF0 RID: 3056
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartSettings_executablePath_get")]
	public static extern string AppStartSettings_executablePath_get(HandleRef jarg1);

	// Token: 0x06000BF1 RID: 3057
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartSettings_workingDir_set")]
	public static extern void AppStartSettings_workingDir_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BF2 RID: 3058
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartSettings_workingDir_get")]
	public static extern string AppStartSettings_workingDir_get(HandleRef jarg1);

	// Token: 0x06000BF3 RID: 3059
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartSettings_commandlineArgs_set")]
	public static extern void AppStartSettings_commandlineArgs_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BF4 RID: 3060
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartSettings_commandlineArgs_get")]
	public static extern string AppStartSettings_commandlineArgs_get(HandleRef jarg1);

	// Token: 0x06000BF5 RID: 3061
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartSettings_renderingAPIs_set")]
	public static extern void AppStartSettings_renderingAPIs_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000BF6 RID: 3062
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartSettings_renderingAPIs_get")]
	public static extern uint AppStartSettings_renderingAPIs_get(HandleRef jarg1);

	// Token: 0x06000BF7 RID: 3063
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartSettings_captureType_set")]
	public static extern void AppStartSettings_captureType_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000BF8 RID: 3064
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartSettings_captureType_get")]
	public static extern uint AppStartSettings_captureType_get(HandleRef jarg1);

	// Token: 0x06000BF9 RID: 3065
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartSettings_envVars_set")]
	public static extern void AppStartSettings_envVars_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BFA RID: 3066
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartSettings_envVars_get")]
	public static extern string AppStartSettings_envVars_get(HandleRef jarg1);

	// Token: 0x06000BFB RID: 3067
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartSettings_launchOptions_set")]
	public static extern void AppStartSettings_launchOptions_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000BFC RID: 3068
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartSettings_launchOptions_get")]
	public static extern string AppStartSettings_launchOptions_get(HandleRef jarg1);

	// Token: 0x06000BFD RID: 3069
	[DllImport("SDPCore", EntryPoint = "CSharp_new_AppStartSettings__SWIG_0")]
	public static extern IntPtr new_AppStartSettings__SWIG_0(string jarg1, string jarg2, string jarg3, uint jarg4, uint jarg5, string jarg6, string jarg7);

	// Token: 0x06000BFE RID: 3070
	[DllImport("SDPCore", EntryPoint = "CSharp_new_AppStartSettings__SWIG_1")]
	public static extern IntPtr new_AppStartSettings__SWIG_1(string jarg1, string jarg2, uint jarg3, uint jarg4, string jarg5);

	// Token: 0x06000BFF RID: 3071
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_AppStartSettings")]
	public static extern void delete_AppStartSettings(HandleRef jarg1);

	// Token: 0x06000C00 RID: 3072
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartResponse_result_set")]
	public static extern void AppStartResponse_result_set(HandleRef jarg1, bool jarg2);

	// Token: 0x06000C01 RID: 3073
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartResponse_result_get")]
	public static extern bool AppStartResponse_result_get(HandleRef jarg1);

	// Token: 0x06000C02 RID: 3074
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartResponse_pid_set")]
	public static extern void AppStartResponse_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000C03 RID: 3075
	[DllImport("SDPCore", EntryPoint = "CSharp_AppStartResponse_pid_get")]
	public static extern uint AppStartResponse_pid_get(HandleRef jarg1);

	// Token: 0x06000C04 RID: 3076
	[DllImport("SDPCore", EntryPoint = "CSharp_new_AppStartResponse__SWIG_0")]
	public static extern IntPtr new_AppStartResponse__SWIG_0(bool jarg1, uint jarg2);

	// Token: 0x06000C05 RID: 3077
	[DllImport("SDPCore", EntryPoint = "CSharp_new_AppStartResponse__SWIG_1")]
	public static extern IntPtr new_AppStartResponse__SWIG_1(bool jarg1);

	// Token: 0x06000C06 RID: 3078
	[DllImport("SDPCore", EntryPoint = "CSharp_new_AppStartResponse__SWIG_2")]
	public static extern IntPtr new_AppStartResponse__SWIG_2();

	// Token: 0x06000C07 RID: 3079
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_AppStartResponse")]
	public static extern void delete_AppStartResponse(HandleRef jarg1);

	// Token: 0x06000C08 RID: 3080
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_DeviceDelegate")]
	public static extern void delete_DeviceDelegate(HandleRef jarg1);

	// Token: 0x06000C09 RID: 3081
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceDelegate_OnDeviceConnected")]
	public static extern void DeviceDelegate_OnDeviceConnected(HandleRef jarg1, string jarg2);

	// Token: 0x06000C0A RID: 3082
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceDelegate_OnDeviceConnectedSwigExplicitDeviceDelegate")]
	public static extern void DeviceDelegate_OnDeviceConnectedSwigExplicitDeviceDelegate(HandleRef jarg1, string jarg2);

	// Token: 0x06000C0B RID: 3083
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceDelegate_OnDeviceDisconnected")]
	public static extern void DeviceDelegate_OnDeviceDisconnected(HandleRef jarg1, string jarg2);

	// Token: 0x06000C0C RID: 3084
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceDelegate_OnDeviceDisconnectedSwigExplicitDeviceDelegate")]
	public static extern void DeviceDelegate_OnDeviceDisconnectedSwigExplicitDeviceDelegate(HandleRef jarg1, string jarg2);

	// Token: 0x06000C0D RID: 3085
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceDelegate_OnDeviceStateChanged")]
	public static extern void DeviceDelegate_OnDeviceStateChanged(HandleRef jarg1, string jarg2);

	// Token: 0x06000C0E RID: 3086
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceDelegate_OnDeviceStateChangedSwigExplicitDeviceDelegate")]
	public static extern void DeviceDelegate_OnDeviceStateChangedSwigExplicitDeviceDelegate(HandleRef jarg1, string jarg2);

	// Token: 0x06000C0F RID: 3087
	[DllImport("SDPCore", EntryPoint = "CSharp_new_DeviceDelegate")]
	public static extern IntPtr new_DeviceDelegate();

	// Token: 0x06000C10 RID: 3088
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceDelegate_director_connect")]
	public static extern void DeviceDelegate_director_connect(HandleRef jarg1, DeviceDelegate.SwigDelegateDeviceDelegate_0 delegate0, DeviceDelegate.SwigDelegateDeviceDelegate_1 delegate1, DeviceDelegate.SwigDelegateDeviceDelegate_2 delegate2);

	// Token: 0x06000C11 RID: 3089
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_Device")]
	public static extern void delete_Device(HandleRef jarg1);

	// Token: 0x06000C12 RID: 3090
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_Init__SWIG_0")]
	public static extern void Device_Init__SWIG_0(HandleRef jarg1, string jarg2);

	// Token: 0x06000C13 RID: 3091
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_Init__SWIG_1")]
	public static extern void Device_Init__SWIG_1(HandleRef jarg1);

	// Token: 0x06000C14 RID: 3092
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_Shutdown")]
	public static extern bool Device_Shutdown(HandleRef jarg1);

	// Token: 0x06000C15 RID: 3093
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_RetryInstall")]
	public static extern void Device_RetryInstall(HandleRef jarg1);

	// Token: 0x06000C16 RID: 3094
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_Connect__SWIG_0")]
	public static extern void Device_Connect__SWIG_0(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000C17 RID: 3095
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_Connect__SWIG_1")]
	public static extern void Device_Connect__SWIG_1(HandleRef jarg1, uint jarg2);

	// Token: 0x06000C18 RID: 3096
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_Connect__SWIG_2")]
	public static extern void Device_Connect__SWIG_2(HandleRef jarg1);

	// Token: 0x06000C19 RID: 3097
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_Disconnect")]
	public static extern void Device_Disconnect(HandleRef jarg1);

	// Token: 0x06000C1A RID: 3098
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_SetDeviceConnectionType")]
	public static extern void Device_SetDeviceConnectionType(HandleRef jarg1, int jarg2);

	// Token: 0x06000C1B RID: 3099
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_GetDeviceState")]
	public static extern int Device_GetDeviceState(HandleRef jarg1);

	// Token: 0x06000C1C RID: 3100
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_GetDeviceConnectionType")]
	public static extern int Device_GetDeviceConnectionType(HandleRef jarg1);

	// Token: 0x06000C1D RID: 3101
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_GetDeviceStateMsg")]
	public static extern string Device_GetDeviceStateMsg(HandleRef jarg1);

	// Token: 0x06000C1E RID: 3102
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_GetCommandNet")]
	public static extern IntPtr Device_GetCommandNet(HandleRef jarg1);

	// Token: 0x06000C1F RID: 3103
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_GetCommandNetPort")]
	public static extern uint Device_GetCommandNetPort(HandleRef jarg1);

	// Token: 0x06000C20 RID: 3104
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_RegisterEventDelegate")]
	public static extern void Device_RegisterEventDelegate(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000C21 RID: 3105
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_DeregisterEventDelegate")]
	public static extern void Device_DeregisterEventDelegate(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000C22 RID: 3106
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_GetName")]
	public static extern string Device_GetName(HandleRef jarg1);

	// Token: 0x06000C23 RID: 3107
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_GetIP")]
	public static extern string Device_GetIP(HandleRef jarg1);

	// Token: 0x06000C24 RID: 3108
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_GetDeviceType")]
	public static extern IntPtr Device_GetDeviceType(HandleRef jarg1);

	// Token: 0x06000C25 RID: 3109
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_GetDeviceAttributes")]
	public static extern IntPtr Device_GetDeviceAttributes(HandleRef jarg1);

	// Token: 0x06000C26 RID: 3110
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_SuspendAllApps")]
	public static extern bool Device_SuspendAllApps(HandleRef jarg1);

	// Token: 0x06000C27 RID: 3111
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_StartApp")]
	public static extern IntPtr Device_StartApp(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000C28 RID: 3112
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_StopApp__SWIG_0")]
	public static extern bool Device_StopApp__SWIG_0(HandleRef jarg1, string jarg2);

	// Token: 0x06000C29 RID: 3113
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_StopApp__SWIG_1")]
	public static extern bool Device_StopApp__SWIG_1(HandleRef jarg1, uint jarg2);

	// Token: 0x06000C2A RID: 3114
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_SetProperty")]
	public static extern bool Device_SetProperty(HandleRef jarg1, int jarg2, string jarg3);

	// Token: 0x06000C2B RID: 3115
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_GetProperty")]
	public static extern string Device_GetProperty(HandleRef jarg1, int jarg2);

	// Token: 0x06000C2C RID: 3116
	[DllImport("SDPCore", EntryPoint = "CSharp_Device_EnableAppPermission")]
	public static extern bool Device_EnableAppPermission(HandleRef jarg1, string jarg2, int jarg3, bool jarg4);

	// Token: 0x06000C2D RID: 3117
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_DeviceManager")]
	public static extern void delete_DeviceManager(HandleRef jarg1);

	// Token: 0x06000C2E RID: 3118
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_Get")]
	public static extern IntPtr DeviceManager_Get();

	// Token: 0x06000C2F RID: 3119
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_Init")]
	public static extern bool DeviceManager_Init(HandleRef jarg1);

	// Token: 0x06000C30 RID: 3120
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_IsInitialized")]
	public static extern bool DeviceManager_IsInitialized(HandleRef jarg1);

	// Token: 0x06000C31 RID: 3121
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_AddDevice")]
	public static extern IntPtr DeviceManager_AddDevice(HandleRef jarg1, string jarg2, string jarg3);

	// Token: 0x06000C32 RID: 3122
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_RemoveDevice")]
	public static extern bool DeviceManager_RemoveDevice(HandleRef jarg1, string jarg2);

	// Token: 0x06000C33 RID: 3123
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_FindDevices")]
	public static extern void DeviceManager_FindDevices(HandleRef jarg1);

	// Token: 0x06000C34 RID: 3124
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetDevices")]
	public static extern IntPtr DeviceManager_GetDevices(HandleRef jarg1);

	// Token: 0x06000C35 RID: 3125
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetDeviceIter")]
	public static extern IntPtr DeviceManager_GetDeviceIter(HandleRef jarg1);

	// Token: 0x06000C36 RID: 3126
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetDevice")]
	public static extern IntPtr DeviceManager_GetDevice(HandleRef jarg1, string jarg2);

	// Token: 0x06000C37 RID: 3127
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetNumKnownDevices")]
	public static extern uint DeviceManager_GetNumKnownDevices(HandleRef jarg1);

	// Token: 0x06000C38 RID: 3128
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_IsConnected")]
	public static extern bool DeviceManager_IsConnected(HandleRef jarg1);

	// Token: 0x06000C39 RID: 3129
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetConnectedDevice")]
	public static extern IntPtr DeviceManager_GetConnectedDevice(HandleRef jarg1);

	// Token: 0x06000C3A RID: 3130
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_RegisterEventDelegate")]
	public static extern void DeviceManager_RegisterEventDelegate(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000C3B RID: 3131
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_UnregisterEventDelegate")]
	public static extern void DeviceManager_UnregisterEventDelegate(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000C3C RID: 3132
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_Shutdown")]
	public static extern void DeviceManager_Shutdown(HandleRef jarg1);

	// Token: 0x06000C3D RID: 3133
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_Reset")]
	public static extern void DeviceManager_Reset(HandleRef jarg1);

	// Token: 0x06000C3E RID: 3134
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetEnvironment")]
	public static extern string DeviceManager_GetEnvironment(HandleRef jarg1);

	// Token: 0x06000C3F RID: 3135
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetQNXIPAddress")]
	public static extern string DeviceManager_GetQNXIPAddress(HandleRef jarg1);

	// Token: 0x06000C40 RID: 3136
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetQNXUsername")]
	public static extern string DeviceManager_GetQNXUsername(HandleRef jarg1);

	// Token: 0x06000C41 RID: 3137
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetQNXPassword")]
	public static extern string DeviceManager_GetQNXPassword(HandleRef jarg1);

	// Token: 0x06000C42 RID: 3138
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetQNXSSHIdentityFile")]
	public static extern string DeviceManager_GetQNXSSHIdentityFile(HandleRef jarg1);

	// Token: 0x06000C43 RID: 3139
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetQNXConnectionType")]
	public static extern string DeviceManager_GetQNXConnectionType(HandleRef jarg1);

	// Token: 0x06000C44 RID: 3140
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetQNXDeployDirectory")]
	public static extern string DeviceManager_GetQNXDeployDirectory(HandleRef jarg1);

	// Token: 0x06000C45 RID: 3141
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetQNXProcessPriority")]
	public static extern int DeviceManager_GetQNXProcessPriority(HandleRef jarg1);

	// Token: 0x06000C46 RID: 3142
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetAGLIPAddress")]
	public static extern string DeviceManager_GetAGLIPAddress(HandleRef jarg1);

	// Token: 0x06000C47 RID: 3143
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetAGLUsername")]
	public static extern string DeviceManager_GetAGLUsername(HandleRef jarg1);

	// Token: 0x06000C48 RID: 3144
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetAGLPassword")]
	public static extern string DeviceManager_GetAGLPassword(HandleRef jarg1);

	// Token: 0x06000C49 RID: 3145
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetAGLDeployDirectory")]
	public static extern string DeviceManager_GetAGLDeployDirectory(HandleRef jarg1);

	// Token: 0x06000C4A RID: 3146
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetWinARMIPAddress")]
	public static extern string DeviceManager_GetWinARMIPAddress(HandleRef jarg1);

	// Token: 0x06000C4B RID: 3147
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetInstallerTimeout")]
	public static extern int DeviceManager_GetInstallerTimeout(HandleRef jarg1);

	// Token: 0x06000C4C RID: 3148
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetLinuxSSHIPAddress")]
	public static extern string DeviceManager_GetLinuxSSHIPAddress(HandleRef jarg1);

	// Token: 0x06000C4D RID: 3149
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetLinuxSSHUsername")]
	public static extern string DeviceManager_GetLinuxSSHUsername(HandleRef jarg1);

	// Token: 0x06000C4E RID: 3150
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetLinuxSSHPassword")]
	public static extern string DeviceManager_GetLinuxSSHPassword(HandleRef jarg1);

	// Token: 0x06000C4F RID: 3151
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetLinuxSSHIdentityFile")]
	public static extern string DeviceManager_GetLinuxSSHIdentityFile(HandleRef jarg1);

	// Token: 0x06000C50 RID: 3152
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetLinuxSSHDeployDirectory")]
	public static extern string DeviceManager_GetLinuxSSHDeployDirectory(HandleRef jarg1);

	// Token: 0x06000C51 RID: 3153
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetSimpleperfPath")]
	public static extern string DeviceManager_GetSimpleperfPath(HandleRef jarg1);

	// Token: 0x06000C52 RID: 3154
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetQNXIPAddress")]
	public static extern bool DeviceManager_SetQNXIPAddress(HandleRef jarg1, string jarg2);

	// Token: 0x06000C53 RID: 3155
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetQNXUsername")]
	public static extern bool DeviceManager_SetQNXUsername(HandleRef jarg1, string jarg2);

	// Token: 0x06000C54 RID: 3156
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetQNXPassword")]
	public static extern bool DeviceManager_SetQNXPassword(HandleRef jarg1, string jarg2);

	// Token: 0x06000C55 RID: 3157
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetQNXSSHIdentityFile")]
	public static extern bool DeviceManager_SetQNXSSHIdentityFile(HandleRef jarg1, string jarg2);

	// Token: 0x06000C56 RID: 3158
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetQNXConnectionType")]
	public static extern bool DeviceManager_SetQNXConnectionType(HandleRef jarg1, string jarg2);

	// Token: 0x06000C57 RID: 3159
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetQNXDeployDirectory")]
	public static extern bool DeviceManager_SetQNXDeployDirectory(HandleRef jarg1, string jarg2);

	// Token: 0x06000C58 RID: 3160
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetQNXProcessPriority")]
	public static extern bool DeviceManager_SetQNXProcessPriority(HandleRef jarg1, int jarg2);

	// Token: 0x06000C59 RID: 3161
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetAGLIPAddress")]
	public static extern bool DeviceManager_SetAGLIPAddress(HandleRef jarg1, string jarg2);

	// Token: 0x06000C5A RID: 3162
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetAGLUsername")]
	public static extern bool DeviceManager_SetAGLUsername(HandleRef jarg1, string jarg2);

	// Token: 0x06000C5B RID: 3163
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetAGLPassword")]
	public static extern bool DeviceManager_SetAGLPassword(HandleRef jarg1, string jarg2);

	// Token: 0x06000C5C RID: 3164
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetAGLDeployDirectory")]
	public static extern bool DeviceManager_SetAGLDeployDirectory(HandleRef jarg1, string jarg2);

	// Token: 0x06000C5D RID: 3165
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetWinARMIPAddress")]
	public static extern bool DeviceManager_SetWinARMIPAddress(HandleRef jarg1, string jarg2);

	// Token: 0x06000C5E RID: 3166
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetInstallerTimeout")]
	public static extern bool DeviceManager_SetInstallerTimeout(HandleRef jarg1, int jarg2);

	// Token: 0x06000C5F RID: 3167
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetLinuxSSHIPAddress")]
	public static extern bool DeviceManager_SetLinuxSSHIPAddress(HandleRef jarg1, string jarg2);

	// Token: 0x06000C60 RID: 3168
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetLinuxSSHUsername")]
	public static extern bool DeviceManager_SetLinuxSSHUsername(HandleRef jarg1, string jarg2);

	// Token: 0x06000C61 RID: 3169
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetLinuxSSHPassword")]
	public static extern bool DeviceManager_SetLinuxSSHPassword(HandleRef jarg1, string jarg2);

	// Token: 0x06000C62 RID: 3170
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetLinuxSSHIdentityFile")]
	public static extern bool DeviceManager_SetLinuxSSHIdentityFile(HandleRef jarg1, string jarg2);

	// Token: 0x06000C63 RID: 3171
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetLinuxSSHDeployDirectory")]
	public static extern bool DeviceManager_SetLinuxSSHDeployDirectory(HandleRef jarg1, string jarg2);

	// Token: 0x06000C64 RID: 3172
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetSimpleperfPath")]
	public static extern bool DeviceManager_SetSimpleperfPath(HandleRef jarg1, string jarg2);

	// Token: 0x06000C65 RID: 3173
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetSimpleperfAvailable")]
	public static extern bool DeviceManager_SetSimpleperfAvailable(HandleRef jarg1, bool jarg2);

	// Token: 0x06000C66 RID: 3174
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_IsSimpleperfAvailable")]
	public static extern bool DeviceManager_IsSimpleperfAvailable(HandleRef jarg1);

	// Token: 0x06000C67 RID: 3175
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetDeleteServiceFilesOnExit")]
	public static extern void DeviceManager_SetDeleteServiceFilesOnExit(HandleRef jarg1, bool jarg2);

	// Token: 0x06000C68 RID: 3176
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetDeleteServiceFilesOnExit")]
	public static extern bool DeviceManager_GetDeleteServiceFilesOnExit(HandleRef jarg1);

	// Token: 0x06000C69 RID: 3177
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_SetHLMIsEnabled")]
	public static extern bool DeviceManager_SetHLMIsEnabled(HandleRef jarg1, bool jarg2);

	// Token: 0x06000C6A RID: 3178
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManager_GetHLMIsEnabled")]
	public static extern bool DeviceManager_GetHLMIsEnabled(HandleRef jarg1);

	// Token: 0x06000C6B RID: 3179
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManagerDelegate_OnDeviceAdded")]
	public static extern void DeviceManagerDelegate_OnDeviceAdded(HandleRef jarg1, string jarg2, string jarg3);

	// Token: 0x06000C6C RID: 3180
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceManagerDelegate_OnDeviceRemoved")]
	public static extern void DeviceManagerDelegate_OnDeviceRemoved(HandleRef jarg1, string jarg2);

	// Token: 0x06000C6D RID: 3181
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_DeviceManagerDelegate")]
	public static extern void delete_DeviceManagerDelegate(HandleRef jarg1);

	// Token: 0x06000C6E RID: 3182
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionSettings_SessionDirectoryRootPath_set")]
	public static extern void SessionSettings_SessionDirectoryRootPath_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000C6F RID: 3183
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionSettings_SessionDirectoryRootPath_get")]
	public static extern string SessionSettings_SessionDirectoryRootPath_get(HandleRef jarg1);

	// Token: 0x06000C70 RID: 3184
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionSettings_MaxTotalSessionsSizeMB_set")]
	public static extern void SessionSettings_MaxTotalSessionsSizeMB_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000C71 RID: 3185
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionSettings_MaxTotalSessionsSizeMB_get")]
	public static extern uint SessionSettings_MaxTotalSessionsSizeMB_get(HandleRef jarg1);

	// Token: 0x06000C72 RID: 3186
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionSettings_CreateTimestampedSubDirectory_set")]
	public static extern void SessionSettings_CreateTimestampedSubDirectory_set(HandleRef jarg1, bool jarg2);

	// Token: 0x06000C73 RID: 3187
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionSettings_CreateTimestampedSubDirectory_get")]
	public static extern bool SessionSettings_CreateTimestampedSubDirectory_get(HandleRef jarg1);

	// Token: 0x06000C74 RID: 3188
	[DllImport("SDPCore", EntryPoint = "CSharp_new_SessionSettings")]
	public static extern IntPtr new_SessionSettings();

	// Token: 0x06000C75 RID: 3189
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_SessionSettings")]
	public static extern void delete_SessionSettings(HandleRef jarg1);

	// Token: 0x06000C76 RID: 3190
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_SessionManager")]
	public static extern void delete_SessionManager(HandleRef jarg1);

	// Token: 0x06000C77 RID: 3191
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionManager_Get")]
	public static extern IntPtr SessionManager_Get();

	// Token: 0x06000C78 RID: 3192
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionManager_OpenSession")]
	public static extern bool SessionManager_OpenSession(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000C79 RID: 3193
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionManager_CloseSession")]
	public static extern bool SessionManager_CloseSession(HandleRef jarg1);

	// Token: 0x06000C7A RID: 3194
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionManager_RegisterEventDelegate")]
	public static extern void SessionManager_RegisterEventDelegate(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000C7B RID: 3195
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionManager_GetSessionPath")]
	public static extern string SessionManager_GetSessionPath(HandleRef jarg1);

	// Token: 0x06000C7C RID: 3196
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionManagerDelegate_OnSessionOpened")]
	public static extern void SessionManagerDelegate_OnSessionOpened(HandleRef jarg1);

	// Token: 0x06000C7D RID: 3197
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionManagerDelegate_OnSessionClosed")]
	public static extern void SessionManagerDelegate_OnSessionClosed(HandleRef jarg1);

	// Token: 0x06000C7E RID: 3198
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_SessionManagerDelegate")]
	public static extern void delete_SessionManagerDelegate(HandleRef jarg1);

	// Token: 0x06000C7F RID: 3199
	[DllImport("SDPCore", EntryPoint = "CSharp_new_CaptureSettings")]
	public static extern IntPtr new_CaptureSettings(uint jarg1, uint jarg2, uint jarg3, uint jarg4, string jarg5);

	// Token: 0x06000C80 RID: 3200
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureSettings_captureType_set")]
	public static extern void CaptureSettings_captureType_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000C81 RID: 3201
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureSettings_captureType_get")]
	public static extern uint CaptureSettings_captureType_get(HandleRef jarg1);

	// Token: 0x06000C82 RID: 3202
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureSettings_processID_set")]
	public static extern void CaptureSettings_processID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000C83 RID: 3203
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureSettings_processID_get")]
	public static extern uint CaptureSettings_processID_get(HandleRef jarg1);

	// Token: 0x06000C84 RID: 3204
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureSettings_startDelay_set")]
	public static extern void CaptureSettings_startDelay_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000C85 RID: 3205
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureSettings_startDelay_get")]
	public static extern uint CaptureSettings_startDelay_get(HandleRef jarg1);

	// Token: 0x06000C86 RID: 3206
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureSettings_duration_set")]
	public static extern void CaptureSettings_duration_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000C87 RID: 3207
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureSettings_duration_get")]
	public static extern uint CaptureSettings_duration_get(HandleRef jarg1);

	// Token: 0x06000C88 RID: 3208
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureSettings_rendererString_set")]
	public static extern void CaptureSettings_rendererString_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000C89 RID: 3209
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureSettings_rendererString_get")]
	public static extern string CaptureSettings_rendererString_get(HandleRef jarg1);

	// Token: 0x06000C8A RID: 3210
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_CaptureSettings")]
	public static extern void delete_CaptureSettings(HandleRef jarg1);

	// Token: 0x06000C8B RID: 3211
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_captureID_set")]
	public static extern void CaptureProperties_captureID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000C8C RID: 3212
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_captureID_get")]
	public static extern uint CaptureProperties_captureID_get(HandleRef jarg1);

	// Token: 0x06000C8D RID: 3213
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_captureType_set")]
	public static extern void CaptureProperties_captureType_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000C8E RID: 3214
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_captureType_get")]
	public static extern uint CaptureProperties_captureType_get(HandleRef jarg1);

	// Token: 0x06000C8F RID: 3215
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_processID_set")]
	public static extern void CaptureProperties_processID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000C90 RID: 3216
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_processID_get")]
	public static extern uint CaptureProperties_processID_get(HandleRef jarg1);

	// Token: 0x06000C91 RID: 3217
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_startTimeTOD_set")]
	public static extern void CaptureProperties_startTimeTOD_set(HandleRef jarg1, long jarg2);

	// Token: 0x06000C92 RID: 3218
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_startTimeTOD_get")]
	public static extern long CaptureProperties_startTimeTOD_get(HandleRef jarg1);

	// Token: 0x06000C93 RID: 3219
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_stopTimeTOD_set")]
	public static extern void CaptureProperties_stopTimeTOD_set(HandleRef jarg1, long jarg2);

	// Token: 0x06000C94 RID: 3220
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_stopTimeTOD_get")]
	public static extern long CaptureProperties_stopTimeTOD_get(HandleRef jarg1);

	// Token: 0x06000C95 RID: 3221
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_startDelay_set")]
	public static extern void CaptureProperties_startDelay_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000C96 RID: 3222
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_startDelay_get")]
	public static extern uint CaptureProperties_startDelay_get(HandleRef jarg1);

	// Token: 0x06000C97 RID: 3223
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_duration_set")]
	public static extern void CaptureProperties_duration_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000C98 RID: 3224
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_duration_get")]
	public static extern uint CaptureProperties_duration_get(HandleRef jarg1);

	// Token: 0x06000C99 RID: 3225
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_rendererString_set")]
	public static extern void CaptureProperties_rendererString_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000C9A RID: 3226
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_rendererString_get")]
	public static extern string CaptureProperties_rendererString_get(HandleRef jarg1);

	// Token: 0x06000C9B RID: 3227
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_captureName_set")]
	public static extern void CaptureProperties_captureName_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000C9C RID: 3228
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureProperties_captureName_get")]
	public static extern string CaptureProperties_captureName_get(HandleRef jarg1);

	// Token: 0x06000C9D RID: 3229
	[DllImport("SDPCore", EntryPoint = "CSharp_new_CaptureProperties")]
	public static extern IntPtr new_CaptureProperties();

	// Token: 0x06000C9E RID: 3230
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_CaptureProperties")]
	public static extern void delete_CaptureProperties(HandleRef jarg1);

	// Token: 0x06000C9F RID: 3231
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureMetric_captureID_set")]
	public static extern void CaptureMetric_captureID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CA0 RID: 3232
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureMetric_captureID_get")]
	public static extern uint CaptureMetric_captureID_get(HandleRef jarg1);

	// Token: 0x06000CA1 RID: 3233
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureMetric_processID_set")]
	public static extern void CaptureMetric_processID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CA2 RID: 3234
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureMetric_processID_get")]
	public static extern uint CaptureMetric_processID_get(HandleRef jarg1);

	// Token: 0x06000CA3 RID: 3235
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureMetric_metricID_set")]
	public static extern void CaptureMetric_metricID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CA4 RID: 3236
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureMetric_metricID_get")]
	public static extern uint CaptureMetric_metricID_get(HandleRef jarg1);

	// Token: 0x06000CA5 RID: 3237
	[DllImport("SDPCore", EntryPoint = "CSharp_new_CaptureMetric")]
	public static extern IntPtr new_CaptureMetric();

	// Token: 0x06000CA6 RID: 3238
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_CaptureMetric")]
	public static extern void delete_CaptureMetric(HandleRef jarg1);

	// Token: 0x06000CA7 RID: 3239
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_Capture")]
	public static extern void delete_Capture(HandleRef jarg1);

	// Token: 0x06000CA8 RID: 3240
	[DllImport("SDPCore", EntryPoint = "CSharp_Capture_Start")]
	public static extern bool Capture_Start(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000CA9 RID: 3241
	[DllImport("SDPCore", EntryPoint = "CSharp_Capture_Stop")]
	public static extern bool Capture_Stop(HandleRef jarg1);

	// Token: 0x06000CAA RID: 3242
	[DllImport("SDPCore", EntryPoint = "CSharp_Capture_Pause__SWIG_0")]
	public static extern bool Capture_Pause__SWIG_0(HandleRef jarg1, bool jarg2);

	// Token: 0x06000CAB RID: 3243
	[DllImport("SDPCore", EntryPoint = "CSharp_Capture_Pause__SWIG_1")]
	public static extern bool Capture_Pause__SWIG_1(HandleRef jarg1);

	// Token: 0x06000CAC RID: 3244
	[DllImport("SDPCore", EntryPoint = "CSharp_Capture_Cancel")]
	public static extern bool Capture_Cancel(HandleRef jarg1);

	// Token: 0x06000CAD RID: 3245
	[DllImport("SDPCore", EntryPoint = "CSharp_Capture_IsValid")]
	public static extern bool Capture_IsValid(HandleRef jarg1);

	// Token: 0x06000CAE RID: 3246
	[DllImport("SDPCore", EntryPoint = "CSharp_Capture_GetProperties")]
	public static extern IntPtr Capture_GetProperties(HandleRef jarg1);

	// Token: 0x06000CAF RID: 3247
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_CaptureManager")]
	public static extern void delete_CaptureManager(HandleRef jarg1);

	// Token: 0x06000CB0 RID: 3248
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureManager_Get")]
	public static extern IntPtr CaptureManager_Get();

	// Token: 0x06000CB1 RID: 3249
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureManager_CreateCapture")]
	public static extern uint CaptureManager_CreateCapture(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CB2 RID: 3250
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureManager_GetCapture__SWIG_0")]
	public static extern IntPtr CaptureManager_GetCapture__SWIG_0(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CB3 RID: 3251
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureManager_GetCapture__SWIG_1")]
	public static extern IntPtr CaptureManager_GetCapture__SWIG_1(HandleRef jarg1);

	// Token: 0x06000CB4 RID: 3252
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureManager_SetCaptureName")]
	public static extern void CaptureManager_SetCaptureName(HandleRef jarg1, uint jarg2, string jarg3);

	// Token: 0x06000CB5 RID: 3253
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureManager_RegisterEventDelegate")]
	public static extern void CaptureManager_RegisterEventDelegate(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000CB6 RID: 3254
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureManager_UnregisterEventDelegate")]
	public static extern void CaptureManager_UnregisterEventDelegate(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000CB7 RID: 3255
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureManager_ShutDown")]
	public static extern void CaptureManager_ShutDown(HandleRef jarg1);

	// Token: 0x06000CB8 RID: 3256
	[DllImport("SDPCore", EntryPoint = "CSharp_NetworkManagerDelegate_OnCmdNetServerCreated")]
	public static extern void NetworkManagerDelegate_OnCmdNetServerCreated(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CB9 RID: 3257
	[DllImport("SDPCore", EntryPoint = "CSharp_NetworkManagerDelegate_OnCmdNetClientCreated")]
	public static extern void NetworkManagerDelegate_OnCmdNetClientCreated(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CBA RID: 3258
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_NetworkManagerDelegate")]
	public static extern void delete_NetworkManagerDelegate(HandleRef jarg1);

	// Token: 0x06000CBB RID: 3259
	[DllImport("SDPCore", EntryPoint = "CSharp_new_NetworkManagerDelegate")]
	public static extern IntPtr new_NetworkManagerDelegate();

	// Token: 0x06000CBC RID: 3260
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_NetworkManager")]
	public static extern void delete_NetworkManager(HandleRef jarg1);

	// Token: 0x06000CBD RID: 3261
	[DllImport("SDPCore", EntryPoint = "CSharp_NetworkManager_Get")]
	public static extern IntPtr NetworkManager_Get();

	// Token: 0x06000CBE RID: 3262
	[DllImport("SDPCore", EntryPoint = "CSharp_NetworkManager_Reset")]
	public static extern void NetworkManager_Reset(HandleRef jarg1);

	// Token: 0x06000CBF RID: 3263
	[DllImport("SDPCore", EntryPoint = "CSharp_NetworkManager_ShutDown")]
	public static extern void NetworkManager_ShutDown(HandleRef jarg1);

	// Token: 0x06000CC0 RID: 3264
	[DllImport("SDPCore", EntryPoint = "CSharp_NetworkManager_RegisterEventDelegate")]
	public static extern void NetworkManager_RegisterEventDelegate(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000CC1 RID: 3265
	[DllImport("SDPCore", EntryPoint = "CSharp_NetworkManager_UnregisterEventDelegate")]
	public static extern void NetworkManager_UnregisterEventDelegate(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000CC2 RID: 3266
	[DllImport("SDPCore", EntryPoint = "CSharp_NetworkManager_AddCmdNetClient")]
	public static extern void NetworkManager_AddCmdNetClient(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000CC3 RID: 3267
	[DllImport("SDPCore", EntryPoint = "CSharp_NetworkManager_AddCmdNetServer")]
	public static extern void NetworkManager_AddCmdNetServer(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000CC4 RID: 3268
	[DllImport("SDPCore", EntryPoint = "CSharp_NetworkManager_GetCmdNetClient")]
	public static extern IntPtr NetworkManager_GetCmdNetClient(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CC5 RID: 3269
	[DllImport("SDPCore", EntryPoint = "CSharp_NetworkManager_GetCmdNetServer")]
	public static extern IntPtr NetworkManager_GetCmdNetServer(HandleRef jarg1);

	// Token: 0x06000CC6 RID: 3270
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ProcessManager")]
	public static extern void delete_ProcessManager(HandleRef jarg1);

	// Token: 0x06000CC7 RID: 3271
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessManager_Get")]
	public static extern IntPtr ProcessManager_Get();

	// Token: 0x06000CC8 RID: 3272
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessManager_DisableDataModel")]
	public static extern void ProcessManager_DisableDataModel(HandleRef jarg1);

	// Token: 0x06000CC9 RID: 3273
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessManager_RegisterEventDelegate")]
	public static extern void ProcessManager_RegisterEventDelegate(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000CCA RID: 3274
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessManager_Reset")]
	public static extern void ProcessManager_Reset(HandleRef jarg1);

	// Token: 0x06000CCB RID: 3275
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessManager_ShutDown")]
	public static extern void ProcessManager_ShutDown(HandleRef jarg1);

	// Token: 0x06000CCC RID: 3276
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessManager_AddProcess")]
	public static extern IntPtr ProcessManager_AddProcess(HandleRef jarg1, uint jarg2, uint jarg3, string jarg4, string jarg5, long jarg6, uint jarg7, uint jarg8, uint jarg9, uint jarg10);

	// Token: 0x06000CCD RID: 3277
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessManager_RemoveProcess")]
	public static extern bool ProcessManager_RemoveProcess(HandleRef jarg1, uint jarg2, long jarg3);

	// Token: 0x06000CCE RID: 3278
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessManager_GetProcess")]
	public static extern IntPtr ProcessManager_GetProcess(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CCF RID: 3279
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessManager_GetProcessByName")]
	public static extern IntPtr ProcessManager_GetProcessByName(HandleRef jarg1, string jarg2);

	// Token: 0x06000CD0 RID: 3280
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessManager_GetAllProcesses")]
	public static extern IntPtr ProcessManager_GetAllProcesses(HandleRef jarg1);

	// Token: 0x06000CD1 RID: 3281
	[DllImport("SDPCore", EntryPoint = "CSharp_LOGGER_TAG_get")]
	public static extern string LOGGER_TAG_get();

	// Token: 0x06000CD2 RID: 3282
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_BROADCAST_get")]
	public static extern uint SDPNET_BROADCAST_get();

	// Token: 0x06000CD3 RID: 3283
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_SERVICE_get")]
	public static extern uint SDPNET_SERVICE_get();

	// Token: 0x06000CD4 RID: 3284
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_CLIENT_get")]
	public static extern uint SDPNET_CLIENT_get();

	// Token: 0x06000CD5 RID: 3285
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_MONITOR_SERVER_get")]
	public static extern uint SDPNET_MONITOR_SERVER_get();

	// Token: 0x06000CD6 RID: 3286
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_ADB_PORT_get")]
	public static extern uint SDPNET_ADB_PORT_get();

	// Token: 0x06000CD7 RID: 3287
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_MONITOR_PORT_get")]
	public static extern uint SDPNET_MONITOR_PORT_get();

	// Token: 0x06000CD8 RID: 3288
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_DEFAULT_COMMAND_PORT_get")]
	public static extern uint SDPNET_DEFAULT_COMMAND_PORT_get();

	// Token: 0x06000CD9 RID: 3289
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPNET_FILE_XFER_PORT_OFFSET_get")]
	public static extern uint SDPNET_FILE_XFER_PORT_OFFSET_get();

	// Token: 0x06000CDA RID: 3290
	[DllImport("SDPCore", EntryPoint = "CSharp_new_DataMessage__SWIG_0")]
	public static extern IntPtr new_DataMessage__SWIG_0(uint jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000CDB RID: 3291
	[DllImport("SDPCore", EntryPoint = "CSharp_new_DataMessage__SWIG_1")]
	public static extern IntPtr new_DataMessage__SWIG_1(uint jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000CDC RID: 3292
	[DllImport("SDPCore", EntryPoint = "CSharp_DataMessage_GetCommandType")]
	public static extern uint DataMessage_GetCommandType(HandleRef jarg1);

	// Token: 0x06000CDD RID: 3293
	[DllImport("SDPCore", EntryPoint = "CSharp_DataMessage_pid_set")]
	public static extern void DataMessage_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CDE RID: 3294
	[DllImport("SDPCore", EntryPoint = "CSharp_DataMessage_pid_get")]
	public static extern uint DataMessage_pid_get(HandleRef jarg1);

	// Token: 0x06000CDF RID: 3295
	[DllImport("SDPCore", EntryPoint = "CSharp_DataMessage_tid_set")]
	public static extern void DataMessage_tid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CE0 RID: 3296
	[DllImport("SDPCore", EntryPoint = "CSharp_DataMessage_tid_get")]
	public static extern uint DataMessage_tid_get(HandleRef jarg1);

	// Token: 0x06000CE1 RID: 3297
	[DllImport("SDPCore", EntryPoint = "CSharp_DataMessage_metricID_set")]
	public static extern void DataMessage_metricID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CE2 RID: 3298
	[DllImport("SDPCore", EntryPoint = "CSharp_DataMessage_metricID_get")]
	public static extern uint DataMessage_metricID_get(HandleRef jarg1);

	// Token: 0x06000CE3 RID: 3299
	[DllImport("SDPCore", EntryPoint = "CSharp_DataMessage_captureID_set")]
	public static extern void DataMessage_captureID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CE4 RID: 3300
	[DllImport("SDPCore", EntryPoint = "CSharp_DataMessage_captureID_get")]
	public static extern uint DataMessage_captureID_get(HandleRef jarg1);

	// Token: 0x06000CE5 RID: 3301
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_DataMessage")]
	public static extern void delete_DataMessage(HandleRef jarg1);

	// Token: 0x06000CE6 RID: 3302
	[DllImport("SDPCore", EntryPoint = "CSharp_new_CommandMsg")]
	public static extern IntPtr new_CommandMsg(uint jarg1);

	// Token: 0x06000CE7 RID: 3303
	[DllImport("SDPCore", EntryPoint = "CSharp_CommandMsg_GetCommandType")]
	public static extern uint CommandMsg_GetCommandType(HandleRef jarg1);

	// Token: 0x06000CE8 RID: 3304
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_CommandMsg")]
	public static extern void delete_CommandMsg(HandleRef jarg1);

	// Token: 0x06000CE9 RID: 3305
	[DllImport("SDPCore", EntryPoint = "CSharp_StartCapture_duration_set")]
	public static extern void StartCapture_duration_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CEA RID: 3306
	[DllImport("SDPCore", EntryPoint = "CSharp_StartCapture_duration_get")]
	public static extern uint StartCapture_duration_get(HandleRef jarg1);

	// Token: 0x06000CEB RID: 3307
	[DllImport("SDPCore", EntryPoint = "CSharp_StartCapture_startDelay_set")]
	public static extern void StartCapture_startDelay_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CEC RID: 3308
	[DllImport("SDPCore", EntryPoint = "CSharp_StartCapture_startDelay_get")]
	public static extern uint StartCapture_startDelay_get(HandleRef jarg1);

	// Token: 0x06000CED RID: 3309
	[DllImport("SDPCore", EntryPoint = "CSharp_StartCapture_captureID_set")]
	public static extern void StartCapture_captureID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CEE RID: 3310
	[DllImport("SDPCore", EntryPoint = "CSharp_StartCapture_captureID_get")]
	public static extern uint StartCapture_captureID_get(HandleRef jarg1);

	// Token: 0x06000CEF RID: 3311
	[DllImport("SDPCore", EntryPoint = "CSharp_StartCapture_captureType_set")]
	public static extern void StartCapture_captureType_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CF0 RID: 3312
	[DllImport("SDPCore", EntryPoint = "CSharp_StartCapture_captureType_get")]
	public static extern uint StartCapture_captureType_get(HandleRef jarg1);

	// Token: 0x06000CF1 RID: 3313
	[DllImport("SDPCore", EntryPoint = "CSharp_new_StartCapture__SWIG_0")]
	public static extern IntPtr new_StartCapture__SWIG_0(uint jarg1, uint jarg2);

	// Token: 0x06000CF2 RID: 3314
	[DllImport("SDPCore", EntryPoint = "CSharp_new_StartCapture__SWIG_1")]
	public static extern IntPtr new_StartCapture__SWIG_1(uint jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000CF3 RID: 3315
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_StartCapture")]
	public static extern void delete_StartCapture(HandleRef jarg1);

	// Token: 0x06000CF4 RID: 3316
	[DllImport("SDPCore", EntryPoint = "CSharp_StartCaptureTimeTOD_captureID_set")]
	public static extern void StartCaptureTimeTOD_captureID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CF5 RID: 3317
	[DllImport("SDPCore", EntryPoint = "CSharp_StartCaptureTimeTOD_captureID_get")]
	public static extern uint StartCaptureTimeTOD_captureID_get(HandleRef jarg1);

	// Token: 0x06000CF6 RID: 3318
	[DllImport("SDPCore", EntryPoint = "CSharp_StartCaptureTimeTOD_startTimeTOD_set")]
	public static extern void StartCaptureTimeTOD_startTimeTOD_set(HandleRef jarg1, long jarg2);

	// Token: 0x06000CF7 RID: 3319
	[DllImport("SDPCore", EntryPoint = "CSharp_StartCaptureTimeTOD_startTimeTOD_get")]
	public static extern long StartCaptureTimeTOD_startTimeTOD_get(HandleRef jarg1);

	// Token: 0x06000CF8 RID: 3320
	[DllImport("SDPCore", EntryPoint = "CSharp_new_StartCaptureTimeTOD")]
	public static extern IntPtr new_StartCaptureTimeTOD(uint jarg1, long jarg2);

	// Token: 0x06000CF9 RID: 3321
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_StartCaptureTimeTOD")]
	public static extern void delete_StartCaptureTimeTOD(HandleRef jarg1);

	// Token: 0x06000CFA RID: 3322
	[DllImport("SDPCore", EntryPoint = "CSharp_StopCapture_captureID_set")]
	public static extern void StopCapture_captureID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CFB RID: 3323
	[DllImport("SDPCore", EntryPoint = "CSharp_StopCapture_captureID_get")]
	public static extern uint StopCapture_captureID_get(HandleRef jarg1);

	// Token: 0x06000CFC RID: 3324
	[DllImport("SDPCore", EntryPoint = "CSharp_new_StopCapture")]
	public static extern IntPtr new_StopCapture(uint jarg1);

	// Token: 0x06000CFD RID: 3325
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_StopCapture")]
	public static extern void delete_StopCapture(HandleRef jarg1);

	// Token: 0x06000CFE RID: 3326
	[DllImport("SDPCore", EntryPoint = "CSharp_StopCaptureTimeTOD_captureID_set")]
	public static extern void StopCaptureTimeTOD_captureID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000CFF RID: 3327
	[DllImport("SDPCore", EntryPoint = "CSharp_StopCaptureTimeTOD_captureID_get")]
	public static extern uint StopCaptureTimeTOD_captureID_get(HandleRef jarg1);

	// Token: 0x06000D00 RID: 3328
	[DllImport("SDPCore", EntryPoint = "CSharp_StopCaptureTimeTOD_stopTimeTOD_set")]
	public static extern void StopCaptureTimeTOD_stopTimeTOD_set(HandleRef jarg1, long jarg2);

	// Token: 0x06000D01 RID: 3329
	[DllImport("SDPCore", EntryPoint = "CSharp_StopCaptureTimeTOD_stopTimeTOD_get")]
	public static extern long StopCaptureTimeTOD_stopTimeTOD_get(HandleRef jarg1);

	// Token: 0x06000D02 RID: 3330
	[DllImport("SDPCore", EntryPoint = "CSharp_new_StopCaptureTimeTOD")]
	public static extern IntPtr new_StopCaptureTimeTOD(uint jarg1, long jarg2);

	// Token: 0x06000D03 RID: 3331
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_StopCaptureTimeTOD")]
	public static extern void delete_StopCaptureTimeTOD(HandleRef jarg1);

	// Token: 0x06000D04 RID: 3332
	[DllImport("SDPCore", EntryPoint = "CSharp_CancelCapture_captureID_set")]
	public static extern void CancelCapture_captureID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D05 RID: 3333
	[DllImport("SDPCore", EntryPoint = "CSharp_CancelCapture_captureID_get")]
	public static extern uint CancelCapture_captureID_get(HandleRef jarg1);

	// Token: 0x06000D06 RID: 3334
	[DllImport("SDPCore", EntryPoint = "CSharp_new_CancelCapture")]
	public static extern IntPtr new_CancelCapture(uint jarg1);

	// Token: 0x06000D07 RID: 3335
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_CancelCapture")]
	public static extern void delete_CancelCapture(HandleRef jarg1);

	// Token: 0x06000D08 RID: 3336
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCaptureComplete_providerID_set")]
	public static extern void ReportCaptureComplete_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D09 RID: 3337
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCaptureComplete_providerID_get")]
	public static extern uint ReportCaptureComplete_providerID_get(HandleRef jarg1);

	// Token: 0x06000D0A RID: 3338
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCaptureComplete_captureID_set")]
	public static extern void ReportCaptureComplete_captureID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D0B RID: 3339
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCaptureComplete_captureID_get")]
	public static extern uint ReportCaptureComplete_captureID_get(HandleRef jarg1);

	// Token: 0x06000D0C RID: 3340
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReportCaptureComplete")]
	public static extern IntPtr new_ReportCaptureComplete(uint jarg1, uint jarg2);

	// Token: 0x06000D0D RID: 3341
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReportCaptureComplete")]
	public static extern void delete_ReportCaptureComplete(HandleRef jarg1);

	// Token: 0x06000D0E RID: 3342
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportBufferRegistered_providerID_set")]
	public static extern void ReportBufferRegistered_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D0F RID: 3343
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportBufferRegistered_providerID_get")]
	public static extern uint ReportBufferRegistered_providerID_get(HandleRef jarg1);

	// Token: 0x06000D10 RID: 3344
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportBufferRegistered_bufferKey_set")]
	public static extern void ReportBufferRegistered_bufferKey_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000D11 RID: 3345
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportBufferRegistered_bufferKey_get")]
	public static extern IntPtr ReportBufferRegistered_bufferKey_get(HandleRef jarg1);

	// Token: 0x06000D12 RID: 3346
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportBufferRegistered_captureID_set")]
	public static extern void ReportBufferRegistered_captureID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D13 RID: 3347
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportBufferRegistered_captureID_get")]
	public static extern uint ReportBufferRegistered_captureID_get(HandleRef jarg1);

	// Token: 0x06000D14 RID: 3348
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReportBufferRegistered")]
	public static extern IntPtr new_ReportBufferRegistered(uint jarg1, uint jarg2, HandleRef jarg3);

	// Token: 0x06000D15 RID: 3349
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReportBufferRegistered")]
	public static extern void delete_ReportBufferRegistered(HandleRef jarg1);

	// Token: 0x06000D16 RID: 3350
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyDataNetAddress_address_set")]
	public static extern void ReplyDataNetAddress_address_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000D17 RID: 3351
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyDataNetAddress_address_get")]
	public static extern string ReplyDataNetAddress_address_get(HandleRef jarg1);

	// Token: 0x06000D18 RID: 3352
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyDataNetAddress_filePort_set")]
	public static extern void ReplyDataNetAddress_filePort_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D19 RID: 3353
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyDataNetAddress_filePort_get")]
	public static extern uint ReplyDataNetAddress_filePort_get(HandleRef jarg1);

	// Token: 0x06000D1A RID: 3354
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyDataNetAddress_optionPort_set")]
	public static extern void ReplyDataNetAddress_optionPort_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D1B RID: 3355
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyDataNetAddress_optionPort_get")]
	public static extern uint ReplyDataNetAddress_optionPort_get(HandleRef jarg1);

	// Token: 0x06000D1C RID: 3356
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyDataNetAddress")]
	public static extern IntPtr new_ReplyDataNetAddress(string jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000D1D RID: 3357
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReplyDataNetAddress")]
	public static extern void delete_ReplyDataNetAddress(HandleRef jarg1);

	// Token: 0x06000D1E RID: 3358
	[DllImport("SDPCore", EntryPoint = "CSharp_new_RequestDataNetAddress")]
	public static extern IntPtr new_RequestDataNetAddress();

	// Token: 0x06000D1F RID: 3359
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_RequestDataNetAddress")]
	public static extern void delete_RequestDataNetAddress(HandleRef jarg1);

	// Token: 0x06000D20 RID: 3360
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReportDeviceMemoryLow")]
	public static extern IntPtr new_ReportDeviceMemoryLow();

	// Token: 0x06000D21 RID: 3361
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReportDeviceMemoryLow")]
	public static extern void delete_ReportDeviceMemoryLow(HandleRef jarg1);

	// Token: 0x06000D22 RID: 3362
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyServiceLocalityData")]
	public static extern IntPtr new_ReplyServiceLocalityData(string jarg1);

	// Token: 0x06000D23 RID: 3363
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyServiceLocalityData_ServiceDataPath")]
	public static extern string ReplyServiceLocalityData_ServiceDataPath(HandleRef jarg1);

	// Token: 0x06000D24 RID: 3364
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReplyServiceLocalityData")]
	public static extern void delete_ReplyServiceLocalityData(HandleRef jarg1);

	// Token: 0x06000D25 RID: 3365
	[DllImport("SDPCore", EntryPoint = "CSharp_new_RequestDataProviderInfo")]
	public static extern IntPtr new_RequestDataProviderInfo();

	// Token: 0x06000D26 RID: 3366
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_RequestDataProviderInfo")]
	public static extern void delete_RequestDataProviderInfo(HandleRef jarg1);

	// Token: 0x06000D27 RID: 3367
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyDataProviderInfo_providerDesc_set")]
	public static extern void ReplyDataProviderInfo_providerDesc_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000D28 RID: 3368
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyDataProviderInfo_providerDesc_get")]
	public static extern IntPtr ReplyDataProviderInfo_providerDesc_get(HandleRef jarg1);

	// Token: 0x06000D29 RID: 3369
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyDataProviderInfo")]
	public static extern IntPtr new_ReplyDataProviderInfo(HandleRef jarg1);

	// Token: 0x06000D2A RID: 3370
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReplyDataProviderInfo")]
	public static extern void delete_ReplyDataProviderInfo(HandleRef jarg1);

	// Token: 0x06000D2B RID: 3371
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_pid_set")]
	public static extern void ProcessAdded_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D2C RID: 3372
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_pid_get")]
	public static extern uint ProcessAdded_pid_get(HandleRef jarg1);

	// Token: 0x06000D2D RID: 3373
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_providerID_set")]
	public static extern void ProcessAdded_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D2E RID: 3374
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_providerID_get")]
	public static extern uint ProcessAdded_providerID_get(HandleRef jarg1);

	// Token: 0x06000D2F RID: 3375
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_isAvailable_set")]
	public static extern void ProcessAdded_isAvailable_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000D30 RID: 3376
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_isAvailable_get")]
	public static extern IntPtr ProcessAdded_isAvailable_get(HandleRef jarg1);

	// Token: 0x06000D31 RID: 3377
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_hasIcon_set")]
	public static extern void ProcessAdded_hasIcon_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000D32 RID: 3378
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_hasIcon_get")]
	public static extern IntPtr ProcessAdded_hasIcon_get(HandleRef jarg1);

	// Token: 0x06000D33 RID: 3379
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_name_set")]
	public static extern void ProcessAdded_name_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000D34 RID: 3380
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_name_get")]
	public static extern string ProcessAdded_name_get(HandleRef jarg1);

	// Token: 0x06000D35 RID: 3381
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_iconMem_set")]
	public static extern void ProcessAdded_iconMem_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000D36 RID: 3382
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_iconMem_get")]
	public static extern IntPtr ProcessAdded_iconMem_get(HandleRef jarg1);

	// Token: 0x06000D37 RID: 3383
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_warningFlagsRealTime_set")]
	public static extern void ProcessAdded_warningFlagsRealTime_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D38 RID: 3384
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_warningFlagsRealTime_get")]
	public static extern uint ProcessAdded_warningFlagsRealTime_get(HandleRef jarg1);

	// Token: 0x06000D39 RID: 3385
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_warningFlagsTrace_set")]
	public static extern void ProcessAdded_warningFlagsTrace_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D3A RID: 3386
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_warningFlagsTrace_get")]
	public static extern uint ProcessAdded_warningFlagsTrace_get(HandleRef jarg1);

	// Token: 0x06000D3B RID: 3387
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_warningFlagsSnapshot_set")]
	public static extern void ProcessAdded_warningFlagsSnapshot_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D3C RID: 3388
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_warningFlagsSnapshot_get")]
	public static extern uint ProcessAdded_warningFlagsSnapshot_get(HandleRef jarg1);

	// Token: 0x06000D3D RID: 3389
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_warningFlagsSampling_set")]
	public static extern void ProcessAdded_warningFlagsSampling_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D3E RID: 3390
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_warningFlagsSampling_get")]
	public static extern uint ProcessAdded_warningFlagsSampling_get(HandleRef jarg1);

	// Token: 0x06000D3F RID: 3391
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessAdded__SWIG_0")]
	public static extern IntPtr new_ProcessAdded__SWIG_0(uint jarg1, string jarg2, HandleRef jarg3, HandleRef jarg4, uint jarg5, HandleRef jarg6, uint jarg7, uint jarg8, uint jarg9, uint jarg10);

	// Token: 0x06000D40 RID: 3392
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessAdded__SWIG_1")]
	public static extern IntPtr new_ProcessAdded__SWIG_1(uint jarg1, string jarg2, HandleRef jarg3, HandleRef jarg4, uint jarg5, HandleRef jarg6, uint jarg7, uint jarg8, uint jarg9);

	// Token: 0x06000D41 RID: 3393
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessAdded__SWIG_2")]
	public static extern IntPtr new_ProcessAdded__SWIG_2(uint jarg1, string jarg2, HandleRef jarg3, HandleRef jarg4, uint jarg5, HandleRef jarg6, uint jarg7, uint jarg8);

	// Token: 0x06000D42 RID: 3394
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessAdded__SWIG_3")]
	public static extern IntPtr new_ProcessAdded__SWIG_3(uint jarg1, string jarg2, HandleRef jarg3, HandleRef jarg4, uint jarg5, HandleRef jarg6, uint jarg7);

	// Token: 0x06000D43 RID: 3395
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessAdded__SWIG_4")]
	public static extern IntPtr new_ProcessAdded__SWIG_4(uint jarg1, string jarg2, HandleRef jarg3, HandleRef jarg4, uint jarg5, HandleRef jarg6);

	// Token: 0x06000D44 RID: 3396
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessAdded__SWIG_5")]
	public static extern IntPtr new_ProcessAdded__SWIG_5(uint jarg1, string jarg2, HandleRef jarg3, HandleRef jarg4, uint jarg5);

	// Token: 0x06000D45 RID: 3397
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessAdded__SWIG_6")]
	public static extern IntPtr new_ProcessAdded__SWIG_6(uint jarg1, string jarg2, HandleRef jarg3, HandleRef jarg4);

	// Token: 0x06000D46 RID: 3398
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ProcessAdded")]
	public static extern void delete_ProcessAdded(HandleRef jarg1);

	// Token: 0x06000D47 RID: 3399
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessRemoved_pid_set")]
	public static extern void ProcessRemoved_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D48 RID: 3400
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessRemoved_pid_get")]
	public static extern uint ProcessRemoved_pid_get(HandleRef jarg1);

	// Token: 0x06000D49 RID: 3401
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessRemoved_providerID_set")]
	public static extern void ProcessRemoved_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D4A RID: 3402
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessRemoved_providerID_get")]
	public static extern uint ProcessRemoved_providerID_get(HandleRef jarg1);

	// Token: 0x06000D4B RID: 3403
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessRemoved__SWIG_0")]
	public static extern IntPtr new_ProcessRemoved__SWIG_0(uint jarg1, uint jarg2);

	// Token: 0x06000D4C RID: 3404
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessRemoved__SWIG_1")]
	public static extern IntPtr new_ProcessRemoved__SWIG_1(uint jarg1);

	// Token: 0x06000D4D RID: 3405
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ProcessRemoved")]
	public static extern void delete_ProcessRemoved(HandleRef jarg1);

	// Token: 0x06000D4E RID: 3406
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestProcessStatus_pid_set")]
	public static extern void RequestProcessStatus_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D4F RID: 3407
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestProcessStatus_pid_get")]
	public static extern uint RequestProcessStatus_pid_get(HandleRef jarg1);

	// Token: 0x06000D50 RID: 3408
	[DllImport("SDPCore", EntryPoint = "CSharp_new_RequestProcessStatus")]
	public static extern IntPtr new_RequestProcessStatus(uint jarg1);

	// Token: 0x06000D51 RID: 3409
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_RequestProcessStatus")]
	public static extern void delete_RequestProcessStatus(HandleRef jarg1);

	// Token: 0x06000D52 RID: 3410
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricCategories_providerID_set")]
	public static extern void RequestMetricCategories_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D53 RID: 3411
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricCategories_providerID_get")]
	public static extern uint RequestMetricCategories_providerID_get(HandleRef jarg1);

	// Token: 0x06000D54 RID: 3412
	[DllImport("SDPCore", EntryPoint = "CSharp_new_RequestMetricCategories")]
	public static extern IntPtr new_RequestMetricCategories(uint jarg1);

	// Token: 0x06000D55 RID: 3413
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_RequestMetricCategories")]
	public static extern void delete_RequestMetricCategories(HandleRef jarg1);

	// Token: 0x06000D56 RID: 3414
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategory_providerID_set")]
	public static extern void ReplyMetricCategory_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D57 RID: 3415
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategory_providerID_get")]
	public static extern uint ReplyMetricCategory_providerID_get(HandleRef jarg1);

	// Token: 0x06000D58 RID: 3416
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategory_categoryID_set")]
	public static extern void ReplyMetricCategory_categoryID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D59 RID: 3417
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategory_categoryID_get")]
	public static extern uint ReplyMetricCategory_categoryID_get(HandleRef jarg1);

	// Token: 0x06000D5A RID: 3418
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategory_parentID_set")]
	public static extern void ReplyMetricCategory_parentID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D5B RID: 3419
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategory_parentID_get")]
	public static extern uint ReplyMetricCategory_parentID_get(HandleRef jarg1);

	// Token: 0x06000D5C RID: 3420
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategory_name_set")]
	public static extern void ReplyMetricCategory_name_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000D5D RID: 3421
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategory_name_get")]
	public static extern string ReplyMetricCategory_name_get(HandleRef jarg1);

	// Token: 0x06000D5E RID: 3422
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategory_description_set")]
	public static extern void ReplyMetricCategory_description_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000D5F RID: 3423
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategory_description_get")]
	public static extern string ReplyMetricCategory_description_get(HandleRef jarg1);

	// Token: 0x06000D60 RID: 3424
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategory_isEnabled_set")]
	public static extern void ReplyMetricCategory_isEnabled_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000D61 RID: 3425
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategory_isEnabled_get")]
	public static extern IntPtr ReplyMetricCategory_isEnabled_get(HandleRef jarg1);

	// Token: 0x06000D62 RID: 3426
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyMetricCategory")]
	public static extern IntPtr new_ReplyMetricCategory(uint jarg1, uint jarg2, uint jarg3, string jarg4, string jarg5, bool jarg6);

	// Token: 0x06000D63 RID: 3427
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReplyMetricCategory")]
	public static extern void delete_ReplyMetricCategory(HandleRef jarg1);

	// Token: 0x06000D64 RID: 3428
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategoryTotal_numCategories_set")]
	public static extern void ReplyMetricCategoryTotal_numCategories_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D65 RID: 3429
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategoryTotal_numCategories_get")]
	public static extern uint ReplyMetricCategoryTotal_numCategories_get(HandleRef jarg1);

	// Token: 0x06000D66 RID: 3430
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyMetricCategoryTotal")]
	public static extern IntPtr new_ReplyMetricCategoryTotal(uint jarg1);

	// Token: 0x06000D67 RID: 3431
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReplyMetricCategoryTotal")]
	public static extern void delete_ReplyMetricCategoryTotal(HandleRef jarg1);

	// Token: 0x06000D68 RID: 3432
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategoryAvailable_categoryID_set")]
	public static extern void ReplyMetricCategoryAvailable_categoryID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D69 RID: 3433
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategoryAvailable_categoryID_get")]
	public static extern uint ReplyMetricCategoryAvailable_categoryID_get(HandleRef jarg1);

	// Token: 0x06000D6A RID: 3434
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategoryAvailable_available_set")]
	public static extern void ReplyMetricCategoryAvailable_available_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000D6B RID: 3435
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategoryAvailable_available_get")]
	public static extern IntPtr ReplyMetricCategoryAvailable_available_get(HandleRef jarg1);

	// Token: 0x06000D6C RID: 3436
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyMetricCategoryAvailable")]
	public static extern IntPtr new_ReplyMetricCategoryAvailable(uint jarg1, bool jarg2);

	// Token: 0x06000D6D RID: 3437
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReplyMetricCategoryAvailable")]
	public static extern void delete_ReplyMetricCategoryAvailable(HandleRef jarg1);

	// Token: 0x06000D6E RID: 3438
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetrics_providerID_set")]
	public static extern void RequestMetrics_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D6F RID: 3439
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetrics_providerID_get")]
	public static extern uint RequestMetrics_providerID_get(HandleRef jarg1);

	// Token: 0x06000D70 RID: 3440
	[DllImport("SDPCore", EntryPoint = "CSharp_new_RequestMetrics")]
	public static extern IntPtr new_RequestMetrics(uint jarg1);

	// Token: 0x06000D71 RID: 3441
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_RequestMetrics")]
	public static extern void delete_RequestMetrics(HandleRef jarg1);

	// Token: 0x06000D72 RID: 3442
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_providerID_set")]
	public static extern void ReplyMetric_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D73 RID: 3443
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_providerID_get")]
	public static extern uint ReplyMetric_providerID_get(HandleRef jarg1);

	// Token: 0x06000D74 RID: 3444
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_metricID_set")]
	public static extern void ReplyMetric_metricID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D75 RID: 3445
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_metricID_get")]
	public static extern uint ReplyMetric_metricID_get(HandleRef jarg1);

	// Token: 0x06000D76 RID: 3446
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_metricCategoryID_set")]
	public static extern void ReplyMetric_metricCategoryID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D77 RID: 3447
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_metricCategoryID_get")]
	public static extern uint ReplyMetric_metricCategoryID_get(HandleRef jarg1);

	// Token: 0x06000D78 RID: 3448
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_isEnabled_set")]
	public static extern void ReplyMetric_isEnabled_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000D79 RID: 3449
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_isEnabled_get")]
	public static extern IntPtr ReplyMetric_isEnabled_get(HandleRef jarg1);

	// Token: 0x06000D7A RID: 3450
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_isAvailable_set")]
	public static extern void ReplyMetric_isAvailable_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000D7B RID: 3451
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_isAvailable_get")]
	public static extern IntPtr ReplyMetric_isAvailable_get(HandleRef jarg1);

	// Token: 0x06000D7C RID: 3452
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_isGlobal_set")]
	public static extern void ReplyMetric_isGlobal_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000D7D RID: 3453
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_isGlobal_get")]
	public static extern IntPtr ReplyMetric_isGlobal_get(HandleRef jarg1);

	// Token: 0x06000D7E RID: 3454
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_dataType_set")]
	public static extern void ReplyMetric_dataType_set(HandleRef jarg1, int jarg2);

	// Token: 0x06000D7F RID: 3455
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_dataType_get")]
	public static extern int ReplyMetric_dataType_get(HandleRef jarg1);

	// Token: 0x06000D80 RID: 3456
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_sampleFrequency_set")]
	public static extern void ReplyMetric_sampleFrequency_set(HandleRef jarg1, float jarg2);

	// Token: 0x06000D81 RID: 3457
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_sampleFrequency_get")]
	public static extern float ReplyMetric_sampleFrequency_get(HandleRef jarg1);

	// Token: 0x06000D82 RID: 3458
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_captureType_set")]
	public static extern void ReplyMetric_captureType_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D83 RID: 3459
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_captureType_get")]
	public static extern uint ReplyMetric_captureType_get(HandleRef jarg1);

	// Token: 0x06000D84 RID: 3460
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_dataRefType_set")]
	public static extern void ReplyMetric_dataRefType_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D85 RID: 3461
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_dataRefType_get")]
	public static extern uint ReplyMetric_dataRefType_get(HandleRef jarg1);

	// Token: 0x06000D86 RID: 3462
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_name_set")]
	public static extern void ReplyMetric_name_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000D87 RID: 3463
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_name_get")]
	public static extern string ReplyMetric_name_get(HandleRef jarg1);

	// Token: 0x06000D88 RID: 3464
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_description_set")]
	public static extern void ReplyMetric_description_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000D89 RID: 3465
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_description_get")]
	public static extern string ReplyMetric_description_get(HandleRef jarg1);

	// Token: 0x06000D8A RID: 3466
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_pid_set")]
	public static extern void ReplyMetric_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D8B RID: 3467
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_pid_get")]
	public static extern uint ReplyMetric_pid_get(HandleRef jarg1);

	// Token: 0x06000D8C RID: 3468
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_hidden_set")]
	public static extern void ReplyMetric_hidden_set(HandleRef jarg1, bool jarg2);

	// Token: 0x06000D8D RID: 3469
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_hidden_get")]
	public static extern bool ReplyMetric_hidden_get(HandleRef jarg1);

	// Token: 0x06000D8E RID: 3470
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_userData_set")]
	public static extern void ReplyMetric_userData_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D8F RID: 3471
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_userData_get")]
	public static extern uint ReplyMetric_userData_get(HandleRef jarg1);

	// Token: 0x06000D90 RID: 3472
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyMetric__SWIG_0")]
	public static extern IntPtr new_ReplyMetric__SWIG_0(uint jarg1, uint jarg2, uint jarg3, string jarg4, string jarg5, bool jarg6, bool jarg7, bool jarg8, int jarg9, float jarg10, uint jarg11, uint jarg12, uint jarg13, bool jarg14, uint jarg15);

	// Token: 0x06000D91 RID: 3473
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyMetric__SWIG_1")]
	public static extern IntPtr new_ReplyMetric__SWIG_1(uint jarg1, uint jarg2, uint jarg3, string jarg4, string jarg5, bool jarg6, bool jarg7, bool jarg8, int jarg9, float jarg10, uint jarg11, uint jarg12, uint jarg13, bool jarg14);

	// Token: 0x06000D92 RID: 3474
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReplyMetric")]
	public static extern void delete_ReplyMetric(HandleRef jarg1);

	// Token: 0x06000D93 RID: 3475
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricTotal_providerID_set")]
	public static extern void ReplyMetricTotal_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D94 RID: 3476
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricTotal_providerID_get")]
	public static extern uint ReplyMetricTotal_providerID_get(HandleRef jarg1);

	// Token: 0x06000D95 RID: 3477
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricTotal_numMetrics_set")]
	public static extern void ReplyMetricTotal_numMetrics_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D96 RID: 3478
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricTotal_numMetrics_get")]
	public static extern uint ReplyMetricTotal_numMetrics_get(HandleRef jarg1);

	// Token: 0x06000D97 RID: 3479
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyMetricTotal")]
	public static extern IntPtr new_ReplyMetricTotal(uint jarg1, uint jarg2);

	// Token: 0x06000D98 RID: 3480
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReplyMetricTotal")]
	public static extern void delete_ReplyMetricTotal(HandleRef jarg1);

	// Token: 0x06000D99 RID: 3481
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricEnable_metricID_set")]
	public static extern void RequestMetricEnable_metricID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D9A RID: 3482
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricEnable_metricID_get")]
	public static extern uint RequestMetricEnable_metricID_get(HandleRef jarg1);

	// Token: 0x06000D9B RID: 3483
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricEnable_processID_set")]
	public static extern void RequestMetricEnable_processID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D9C RID: 3484
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricEnable_processID_get")]
	public static extern uint RequestMetricEnable_processID_get(HandleRef jarg1);

	// Token: 0x06000D9D RID: 3485
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricEnable_captureState_set")]
	public static extern void RequestMetricEnable_captureState_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000D9E RID: 3486
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricEnable_captureState_get")]
	public static extern uint RequestMetricEnable_captureState_get(HandleRef jarg1);

	// Token: 0x06000D9F RID: 3487
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricEnable_enable_set")]
	public static extern void RequestMetricEnable_enable_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000DA0 RID: 3488
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricEnable_enable_get")]
	public static extern IntPtr RequestMetricEnable_enable_get(HandleRef jarg1);

	// Token: 0x06000DA1 RID: 3489
	[DllImport("SDPCore", EntryPoint = "CSharp_new_RequestMetricEnable")]
	public static extern IntPtr new_RequestMetricEnable(uint jarg1, uint jarg2, uint jarg3, bool jarg4);

	// Token: 0x06000DA2 RID: 3490
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_RequestMetricEnable")]
	public static extern void delete_RequestMetricEnable(HandleRef jarg1);

	// Token: 0x06000DA3 RID: 3491
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricStatus_metricID_set")]
	public static extern void RequestMetricStatus_metricID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DA4 RID: 3492
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricStatus_metricID_get")]
	public static extern uint RequestMetricStatus_metricID_get(HandleRef jarg1);

	// Token: 0x06000DA5 RID: 3493
	[DllImport("SDPCore", EntryPoint = "CSharp_new_RequestMetricStatus")]
	public static extern IntPtr new_RequestMetricStatus(uint jarg1);

	// Token: 0x06000DA6 RID: 3494
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_RequestMetricStatus")]
	public static extern void delete_RequestMetricStatus(HandleRef jarg1);

	// Token: 0x06000DA7 RID: 3495
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricStatus_metricID_set")]
	public static extern void ReplyMetricStatus_metricID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DA8 RID: 3496
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricStatus_metricID_get")]
	public static extern uint ReplyMetricStatus_metricID_get(HandleRef jarg1);

	// Token: 0x06000DA9 RID: 3497
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricStatus_status_set")]
	public static extern void ReplyMetricStatus_status_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000DAA RID: 3498
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricStatus_status_get")]
	public static extern IntPtr ReplyMetricStatus_status_get(HandleRef jarg1);

	// Token: 0x06000DAB RID: 3499
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyMetricStatus")]
	public static extern IntPtr new_ReplyMetricStatus(uint jarg1, bool jarg2);

	// Token: 0x06000DAC RID: 3500
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReplyMetricStatus")]
	public static extern void delete_ReplyMetricStatus(HandleRef jarg1);

	// Token: 0x06000DAD RID: 3501
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricAvailable_metricID_set")]
	public static extern void ReplyMetricAvailable_metricID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DAE RID: 3502
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricAvailable_metricID_get")]
	public static extern uint ReplyMetricAvailable_metricID_get(HandleRef jarg1);

	// Token: 0x06000DAF RID: 3503
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricAvailable_available_set")]
	public static extern void ReplyMetricAvailable_available_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000DB0 RID: 3504
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricAvailable_available_get")]
	public static extern IntPtr ReplyMetricAvailable_available_get(HandleRef jarg1);

	// Token: 0x06000DB1 RID: 3505
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyMetricAvailable")]
	public static extern IntPtr new_ReplyMetricAvailable(uint jarg1, bool jarg2);

	// Token: 0x06000DB2 RID: 3506
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReplyMetricAvailable")]
	public static extern void delete_ReplyMetricAvailable(HandleRef jarg1);

	// Token: 0x06000DB3 RID: 3507
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricHidden_metricID_set")]
	public static extern void RequestMetricHidden_metricID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DB4 RID: 3508
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricHidden_metricID_get")]
	public static extern uint RequestMetricHidden_metricID_get(HandleRef jarg1);

	// Token: 0x06000DB5 RID: 3509
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricHidden_hidden_set")]
	public static extern void RequestMetricHidden_hidden_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000DB6 RID: 3510
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricHidden_hidden_get")]
	public static extern IntPtr RequestMetricHidden_hidden_get(HandleRef jarg1);

	// Token: 0x06000DB7 RID: 3511
	[DllImport("SDPCore", EntryPoint = "CSharp_new_RequestMetricHidden")]
	public static extern IntPtr new_RequestMetricHidden(uint jarg1, bool jarg2);

	// Token: 0x06000DB8 RID: 3512
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_RequestMetricHidden")]
	public static extern void delete_RequestMetricHidden(HandleRef jarg1);

	// Token: 0x06000DB9 RID: 3513
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestLaunchApp_executablePath_set")]
	public static extern void RequestLaunchApp_executablePath_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000DBA RID: 3514
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestLaunchApp_executablePath_get")]
	public static extern string RequestLaunchApp_executablePath_get(HandleRef jarg1);

	// Token: 0x06000DBB RID: 3515
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestLaunchApp_workingDirectory_set")]
	public static extern void RequestLaunchApp_workingDirectory_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000DBC RID: 3516
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestLaunchApp_workingDirectory_get")]
	public static extern string RequestLaunchApp_workingDirectory_get(HandleRef jarg1);

	// Token: 0x06000DBD RID: 3517
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestLaunchApp_commandlineArguments_set")]
	public static extern void RequestLaunchApp_commandlineArguments_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000DBE RID: 3518
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestLaunchApp_commandlineArguments_get")]
	public static extern string RequestLaunchApp_commandlineArguments_get(HandleRef jarg1);

	// Token: 0x06000DBF RID: 3519
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestLaunchApp_renderingAPIs_set")]
	public static extern void RequestLaunchApp_renderingAPIs_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000DC0 RID: 3520
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestLaunchApp_renderingAPIs_get")]
	public static extern IntPtr RequestLaunchApp_renderingAPIs_get(HandleRef jarg1);

	// Token: 0x06000DC1 RID: 3521
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestLaunchApp_captureType_set")]
	public static extern void RequestLaunchApp_captureType_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DC2 RID: 3522
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestLaunchApp_captureType_get")]
	public static extern uint RequestLaunchApp_captureType_get(HandleRef jarg1);

	// Token: 0x06000DC3 RID: 3523
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestLaunchApp_environmentVariables_set")]
	public static extern void RequestLaunchApp_environmentVariables_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000DC4 RID: 3524
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestLaunchApp_environmentVariables_get")]
	public static extern string RequestLaunchApp_environmentVariables_get(HandleRef jarg1);

	// Token: 0x06000DC5 RID: 3525
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestLaunchApp_appLaunchOptions_set")]
	public static extern void RequestLaunchApp_appLaunchOptions_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000DC6 RID: 3526
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestLaunchApp_appLaunchOptions_get")]
	public static extern string RequestLaunchApp_appLaunchOptions_get(HandleRef jarg1);

	// Token: 0x06000DC7 RID: 3527
	[DllImport("SDPCore", EntryPoint = "CSharp_new_RequestLaunchApp")]
	public static extern IntPtr new_RequestLaunchApp(string jarg1, string jarg2, string jarg3, uint jarg4, uint jarg5, string jarg6, string jarg7);

	// Token: 0x06000DC8 RID: 3528
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_RequestLaunchApp")]
	public static extern void delete_RequestLaunchApp(HandleRef jarg1);

	// Token: 0x06000DC9 RID: 3529
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyLaunchApp_result_set")]
	public static extern void ReplyLaunchApp_result_set(HandleRef jarg1, bool jarg2);

	// Token: 0x06000DCA RID: 3530
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyLaunchApp_result_get")]
	public static extern bool ReplyLaunchApp_result_get(HandleRef jarg1);

	// Token: 0x06000DCB RID: 3531
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyLaunchApp_pid_set")]
	public static extern void ReplyLaunchApp_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DCC RID: 3532
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyLaunchApp_pid_get")]
	public static extern uint ReplyLaunchApp_pid_get(HandleRef jarg1);

	// Token: 0x06000DCD RID: 3533
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyLaunchApp__SWIG_0")]
	public static extern IntPtr new_ReplyLaunchApp__SWIG_0(bool jarg1, uint jarg2);

	// Token: 0x06000DCE RID: 3534
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyLaunchApp__SWIG_1")]
	public static extern IntPtr new_ReplyLaunchApp__SWIG_1(bool jarg1);

	// Token: 0x06000DCF RID: 3535
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReplyLaunchApp")]
	public static extern void delete_ReplyLaunchApp(HandleRef jarg1);

	// Token: 0x06000DD0 RID: 3536
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestStopApp_processID_set")]
	public static extern void RequestStopApp_processID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DD1 RID: 3537
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestStopApp_processID_get")]
	public static extern uint RequestStopApp_processID_get(HandleRef jarg1);

	// Token: 0x06000DD2 RID: 3538
	[DllImport("SDPCore", EntryPoint = "CSharp_new_RequestStopApp")]
	public static extern IntPtr new_RequestStopApp(uint jarg1);

	// Token: 0x06000DD3 RID: 3539
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_RequestStopApp")]
	public static extern void delete_RequestStopApp(HandleRef jarg1);

	// Token: 0x06000DD4 RID: 3540
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyStopApp_result_set")]
	public static extern void ReplyStopApp_result_set(HandleRef jarg1, bool jarg2);

	// Token: 0x06000DD5 RID: 3541
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyStopApp_result_get")]
	public static extern bool ReplyStopApp_result_get(HandleRef jarg1);

	// Token: 0x06000DD6 RID: 3542
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyStopApp")]
	public static extern IntPtr new_ReplyStopApp(bool jarg1);

	// Token: 0x06000DD7 RID: 3543
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReplyStopApp")]
	public static extern void delete_ReplyStopApp(HandleRef jarg1);

	// Token: 0x06000DD8 RID: 3544
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestCustomData_providerID_set")]
	public static extern void RequestCustomData_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DD9 RID: 3545
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestCustomData_providerID_get")]
	public static extern uint RequestCustomData_providerID_get(HandleRef jarg1);

	// Token: 0x06000DDA RID: 3546
	[DllImport("SDPCore", EntryPoint = "CSharp_new_RequestCustomData")]
	public static extern IntPtr new_RequestCustomData(uint jarg1);

	// Token: 0x06000DDB RID: 3547
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_RequestCustomData")]
	public static extern void delete_RequestCustomData(HandleRef jarg1);

	// Token: 0x06000DDC RID: 3548
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomData_providerID_set")]
	public static extern void ReportCustomData_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DDD RID: 3549
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomData_providerID_get")]
	public static extern uint ReportCustomData_providerID_get(HandleRef jarg1);

	// Token: 0x06000DDE RID: 3550
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomData_uid_set")]
	public static extern void ReportCustomData_uid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DDF RID: 3551
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomData_uid_get")]
	public static extern uint ReportCustomData_uid_get(HandleRef jarg1);

	// Token: 0x06000DE0 RID: 3552
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomData_numCustomData_set")]
	public static extern void ReportCustomData_numCustomData_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DE1 RID: 3553
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomData_numCustomData_get")]
	public static extern uint ReportCustomData_numCustomData_get(HandleRef jarg1);

	// Token: 0x06000DE2 RID: 3554
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomData_name_set")]
	public static extern void ReportCustomData_name_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000DE3 RID: 3555
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomData_name_get")]
	public static extern string ReportCustomData_name_get(HandleRef jarg1);

	// Token: 0x06000DE4 RID: 3556
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReportCustomData")]
	public static extern IntPtr new_ReportCustomData(uint jarg1, uint jarg2, string jarg3, uint jarg4);

	// Token: 0x06000DE5 RID: 3557
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReportCustomData")]
	public static extern void delete_ReportCustomData(HandleRef jarg1);

	// Token: 0x06000DE6 RID: 3558
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataBind_providerID_set")]
	public static extern void ReportCustomDataBind_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DE7 RID: 3559
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataBind_providerID_get")]
	public static extern uint ReportCustomDataBind_providerID_get(HandleRef jarg1);

	// Token: 0x06000DE8 RID: 3560
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataBind_uid_set")]
	public static extern void ReportCustomDataBind_uid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DE9 RID: 3561
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataBind_uid_get")]
	public static extern uint ReportCustomDataBind_uid_get(HandleRef jarg1);

	// Token: 0x06000DEA RID: 3562
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataBind_metricID_set")]
	public static extern void ReportCustomDataBind_metricID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DEB RID: 3563
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataBind_metricID_get")]
	public static extern uint ReportCustomDataBind_metricID_get(HandleRef jarg1);

	// Token: 0x06000DEC RID: 3564
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReportCustomDataBind")]
	public static extern IntPtr new_ReportCustomDataBind(uint jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000DED RID: 3565
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReportCustomDataBind")]
	public static extern void delete_ReportCustomDataBind(HandleRef jarg1);

	// Token: 0x06000DEE RID: 3566
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataAttribute_providerID_set")]
	public static extern void ReportCustomDataAttribute_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DEF RID: 3567
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataAttribute_providerID_get")]
	public static extern uint ReportCustomDataAttribute_providerID_get(HandleRef jarg1);

	// Token: 0x06000DF0 RID: 3568
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataAttribute_uid_set")]
	public static extern void ReportCustomDataAttribute_uid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DF1 RID: 3569
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataAttribute_uid_get")]
	public static extern uint ReportCustomDataAttribute_uid_get(HandleRef jarg1);

	// Token: 0x06000DF2 RID: 3570
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataAttribute_attrIdx_set")]
	public static extern void ReportCustomDataAttribute_attrIdx_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DF3 RID: 3571
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataAttribute_attrIdx_get")]
	public static extern uint ReportCustomDataAttribute_attrIdx_get(HandleRef jarg1);

	// Token: 0x06000DF4 RID: 3572
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataAttribute_offset_set")]
	public static extern void ReportCustomDataAttribute_offset_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DF5 RID: 3573
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataAttribute_offset_get")]
	public static extern uint ReportCustomDataAttribute_offset_get(HandleRef jarg1);

	// Token: 0x06000DF6 RID: 3574
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataAttribute_type_set")]
	public static extern void ReportCustomDataAttribute_type_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DF7 RID: 3575
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataAttribute_type_get")]
	public static extern uint ReportCustomDataAttribute_type_get(HandleRef jarg1);

	// Token: 0x06000DF8 RID: 3576
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataAttribute_numAttr_set")]
	public static extern void ReportCustomDataAttribute_numAttr_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DF9 RID: 3577
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataAttribute_numAttr_get")]
	public static extern uint ReportCustomDataAttribute_numAttr_get(HandleRef jarg1);

	// Token: 0x06000DFA RID: 3578
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataAttribute_strName_set")]
	public static extern void ReportCustomDataAttribute_strName_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000DFB RID: 3579
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataAttribute_strName_get")]
	public static extern string ReportCustomDataAttribute_strName_get(HandleRef jarg1);

	// Token: 0x06000DFC RID: 3580
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReportCustomDataAttribute")]
	public static extern IntPtr new_ReportCustomDataAttribute(uint jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, string jarg6, uint jarg7);

	// Token: 0x06000DFD RID: 3581
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReportCustomDataAttribute")]
	public static extern void delete_ReportCustomDataAttribute(HandleRef jarg1);

	// Token: 0x06000DFE RID: 3582
	[DllImport("SDPCore", EntryPoint = "CSharp_ICPStringPairMessage_uid_set")]
	public static extern void ICPStringPairMessage_uid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000DFF RID: 3583
	[DllImport("SDPCore", EntryPoint = "CSharp_ICPStringPairMessage_uid_get")]
	public static extern uint ICPStringPairMessage_uid_get(HandleRef jarg1);

	// Token: 0x06000E00 RID: 3584
	[DllImport("SDPCore", EntryPoint = "CSharp_ICPStringPairMessage_value_set")]
	public static extern void ICPStringPairMessage_value_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000E01 RID: 3585
	[DllImport("SDPCore", EntryPoint = "CSharp_ICPStringPairMessage_value_get")]
	public static extern string ICPStringPairMessage_value_get(HandleRef jarg1);

	// Token: 0x06000E02 RID: 3586
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ICPStringPairMessage")]
	public static extern IntPtr new_ICPStringPairMessage(uint jarg1, string jarg2);

	// Token: 0x06000E03 RID: 3587
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ICPStringPairMessage")]
	public static extern void delete_ICPStringPairMessage(HandleRef jarg1);

	// Token: 0x06000E04 RID: 3588
	[DllImport("SDPCore", EntryPoint = "CSharp_UpdateServiceMsg_copyFinished_set")]
	public static extern void UpdateServiceMsg_copyFinished_set(HandleRef jarg1, bool jarg2);

	// Token: 0x06000E05 RID: 3589
	[DllImport("SDPCore", EntryPoint = "CSharp_UpdateServiceMsg_copyFinished_get")]
	public static extern bool UpdateServiceMsg_copyFinished_get(HandleRef jarg1);

	// Token: 0x06000E06 RID: 3590
	[DllImport("SDPCore", EntryPoint = "CSharp_UpdateServiceMsg_success_set")]
	public static extern void UpdateServiceMsg_success_set(HandleRef jarg1, bool jarg2);

	// Token: 0x06000E07 RID: 3591
	[DllImport("SDPCore", EntryPoint = "CSharp_UpdateServiceMsg_success_get")]
	public static extern bool UpdateServiceMsg_success_get(HandleRef jarg1);

	// Token: 0x06000E08 RID: 3592
	[DllImport("SDPCore", EntryPoint = "CSharp_new_UpdateServiceMsg")]
	public static extern IntPtr new_UpdateServiceMsg(bool jarg1, bool jarg2);

	// Token: 0x06000E09 RID: 3593
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_UpdateServiceMsg")]
	public static extern void delete_UpdateServiceMsg(HandleRef jarg1);

	// Token: 0x06000E0A RID: 3594
	[DllImport("SDPCore", EntryPoint = "CSharp_new_CoreObject")]
	public static extern IntPtr new_CoreObject();

	// Token: 0x06000E0B RID: 3595
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_CoreObject")]
	public static extern void delete_CoreObject(HandleRef jarg1);

	// Token: 0x06000E0C RID: 3596
	[DllImport("SDPCore", EntryPoint = "CSharp_CoreObject_GetID")]
	public static extern uint CoreObject_GetID(HandleRef jarg1);

	// Token: 0x06000E0D RID: 3597
	[DllImport("SDPCore", EntryPoint = "CSharp_CoreObject_GetSystemTimeUS")]
	public static extern long CoreObject_GetSystemTimeUS();

	// Token: 0x06000E0E RID: 3598
	[DllImport("SDPCore", EntryPoint = "CSharp_CoreObject_GetSystemTimeMS")]
	public static extern long CoreObject_GetSystemTimeMS();

	// Token: 0x06000E0F RID: 3599
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_CommonObject")]
	public static extern void delete_CommonObject(HandleRef jarg1);

	// Token: 0x06000E10 RID: 3600
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_GetID")]
	public static extern uint CommonObject_GetID(HandleRef jarg1);

	// Token: 0x06000E11 RID: 3601
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_SetName")]
	public static extern void CommonObject_SetName(HandleRef jarg1, string jarg2);

	// Token: 0x06000E12 RID: 3602
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_GetName")]
	public static extern string CommonObject_GetName(HandleRef jarg1);

	// Token: 0x06000E13 RID: 3603
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_SetDescription")]
	public static extern void CommonObject_SetDescription(HandleRef jarg1, string jarg2);

	// Token: 0x06000E14 RID: 3604
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_GetDescription")]
	public static extern string CommonObject_GetDescription(HandleRef jarg1);

	// Token: 0x06000E15 RID: 3605
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_SetAvailable__SWIG_0")]
	public static extern void CommonObject_SetAvailable__SWIG_0(HandleRef jarg1, bool jarg2);

	// Token: 0x06000E16 RID: 3606
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_SetAvailable__SWIG_1")]
	public static extern void CommonObject_SetAvailable__SWIG_1(HandleRef jarg1);

	// Token: 0x06000E17 RID: 3607
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_IsAvailable")]
	public static extern bool CommonObject_IsAvailable(HandleRef jarg1);

	// Token: 0x06000E18 RID: 3608
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_IsEnabled")]
	public static extern bool CommonObject_IsEnabled(HandleRef jarg1);

	// Token: 0x06000E19 RID: 3609
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_Enable__SWIG_0")]
	public static extern bool CommonObject_Enable__SWIG_0(HandleRef jarg1, bool jarg2);

	// Token: 0x06000E1A RID: 3610
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_Enable__SWIG_1")]
	public static extern bool CommonObject_Enable__SWIG_1(HandleRef jarg1);

	// Token: 0x06000E1B RID: 3611
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_Disable__SWIG_0")]
	public static extern bool CommonObject_Disable__SWIG_0(HandleRef jarg1, bool jarg2);

	// Token: 0x06000E1C RID: 3612
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_Disable__SWIG_1")]
	public static extern bool CommonObject_Disable__SWIG_1(HandleRef jarg1);

	// Token: 0x06000E1D RID: 3613
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_Publish")]
	public static extern bool CommonObject_Publish(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000E1E RID: 3614
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_PublishStatus")]
	public static extern bool CommonObject_PublishStatus(HandleRef jarg1);

	// Token: 0x06000E1F RID: 3615
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_GetSystemTimeUS")]
	public static extern long CommonObject_GetSystemTimeUS(HandleRef jarg1);

	// Token: 0x06000E20 RID: 3616
	[DllImport("SDPCore", EntryPoint = "CSharp_CommonObject_GetSystemTimeMS")]
	public static extern long CommonObject_GetSystemTimeMS(HandleRef jarg1);

	// Token: 0x06000E21 RID: 3617
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessIcon_data_set")]
	public static extern void ProcessIcon_data_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000E22 RID: 3618
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessIcon_data_get")]
	public static extern IntPtr ProcessIcon_data_get(HandleRef jarg1);

	// Token: 0x06000E23 RID: 3619
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessIcon")]
	public static extern IntPtr new_ProcessIcon();

	// Token: 0x06000E24 RID: 3620
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ProcessIcon")]
	public static extern void delete_ProcessIcon(HandleRef jarg1);

	// Token: 0x06000E25 RID: 3621
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_uid_set")]
	public static extern void ProcessProperties_uid_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000E26 RID: 3622
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_uid_get")]
	public static extern string ProcessProperties_uid_get(HandleRef jarg1);

	// Token: 0x06000E27 RID: 3623
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_name_set")]
	public static extern void ProcessProperties_name_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000E28 RID: 3624
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_name_get")]
	public static extern string ProcessProperties_name_get(HandleRef jarg1);

	// Token: 0x06000E29 RID: 3625
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_state_set")]
	public static extern void ProcessProperties_state_set(HandleRef jarg1, int jarg2);

	// Token: 0x06000E2A RID: 3626
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_state_get")]
	public static extern int ProcessProperties_state_get(HandleRef jarg1);

	// Token: 0x06000E2B RID: 3627
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_icon_set")]
	public static extern void ProcessProperties_icon_set(HandleRef jarg1, long jarg2);

	// Token: 0x06000E2C RID: 3628
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_icon_get")]
	public static extern long ProcessProperties_icon_get(HandleRef jarg1);

	// Token: 0x06000E2D RID: 3629
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_pid_set")]
	public static extern void ProcessProperties_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E2E RID: 3630
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_pid_get")]
	public static extern uint ProcessProperties_pid_get(HandleRef jarg1);

	// Token: 0x06000E2F RID: 3631
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_ppid_set")]
	public static extern void ProcessProperties_ppid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E30 RID: 3632
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_ppid_get")]
	public static extern uint ProcessProperties_ppid_get(HandleRef jarg1);

	// Token: 0x06000E31 RID: 3633
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_created_set")]
	public static extern void ProcessProperties_created_set(HandleRef jarg1, long jarg2);

	// Token: 0x06000E32 RID: 3634
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_created_get")]
	public static extern long ProcessProperties_created_get(HandleRef jarg1);

	// Token: 0x06000E33 RID: 3635
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_lastUpdated_set")]
	public static extern void ProcessProperties_lastUpdated_set(HandleRef jarg1, long jarg2);

	// Token: 0x06000E34 RID: 3636
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_lastUpdated_get")]
	public static extern long ProcessProperties_lastUpdated_get(HandleRef jarg1);

	// Token: 0x06000E35 RID: 3637
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_warningFlagsRealTime_set")]
	public static extern void ProcessProperties_warningFlagsRealTime_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E36 RID: 3638
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_warningFlagsRealTime_get")]
	public static extern uint ProcessProperties_warningFlagsRealTime_get(HandleRef jarg1);

	// Token: 0x06000E37 RID: 3639
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_warningFlagsTrace_set")]
	public static extern void ProcessProperties_warningFlagsTrace_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E38 RID: 3640
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_warningFlagsTrace_get")]
	public static extern uint ProcessProperties_warningFlagsTrace_get(HandleRef jarg1);

	// Token: 0x06000E39 RID: 3641
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_warningFlagsSnapshot_set")]
	public static extern void ProcessProperties_warningFlagsSnapshot_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E3A RID: 3642
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_warningFlagsSnapshot_get")]
	public static extern uint ProcessProperties_warningFlagsSnapshot_get(HandleRef jarg1);

	// Token: 0x06000E3B RID: 3643
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_warningFlagsSampling_set")]
	public static extern void ProcessProperties_warningFlagsSampling_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E3C RID: 3644
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProperties_warningFlagsSampling_get")]
	public static extern uint ProcessProperties_warningFlagsSampling_get(HandleRef jarg1);

	// Token: 0x06000E3D RID: 3645
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessProperties")]
	public static extern IntPtr new_ProcessProperties();

	// Token: 0x06000E3E RID: 3646
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ProcessProperties")]
	public static extern void delete_ProcessProperties(HandleRef jarg1);

	// Token: 0x06000E3F RID: 3647
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProviderLink_pid_set")]
	public static extern void ProcessProviderLink_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E40 RID: 3648
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProviderLink_pid_get")]
	public static extern uint ProcessProviderLink_pid_get(HandleRef jarg1);

	// Token: 0x06000E41 RID: 3649
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProviderLink_providerID_set")]
	public static extern void ProcessProviderLink_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E42 RID: 3650
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessProviderLink_providerID_get")]
	public static extern uint ProcessProviderLink_providerID_get(HandleRef jarg1);

	// Token: 0x06000E43 RID: 3651
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessProviderLink")]
	public static extern IntPtr new_ProcessProviderLink();

	// Token: 0x06000E44 RID: 3652
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ProcessProviderLink")]
	public static extern void delete_ProcessProviderLink(HandleRef jarg1);

	// Token: 0x06000E45 RID: 3653
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessMetricLink_pid_set")]
	public static extern void ProcessMetricLink_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E46 RID: 3654
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessMetricLink_pid_get")]
	public static extern uint ProcessMetricLink_pid_get(HandleRef jarg1);

	// Token: 0x06000E47 RID: 3655
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessMetricLink_metricID_set")]
	public static extern void ProcessMetricLink_metricID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E48 RID: 3656
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessMetricLink_metricID_get")]
	public static extern uint ProcessMetricLink_metricID_get(HandleRef jarg1);

	// Token: 0x06000E49 RID: 3657
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessMetricLink")]
	public static extern IntPtr new_ProcessMetricLink();

	// Token: 0x06000E4A RID: 3658
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ProcessMetricLink")]
	public static extern void delete_ProcessMetricLink(HandleRef jarg1);

	// Token: 0x06000E4B RID: 3659
	[DllImport("SDPCore", EntryPoint = "CSharp_new_Process")]
	public static extern IntPtr new_Process(HandleRef jarg1);

	// Token: 0x06000E4C RID: 3660
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_Process")]
	public static extern void delete_Process(HandleRef jarg1);

	// Token: 0x06000E4D RID: 3661
	[DllImport("SDPCore", EntryPoint = "CSharp_Process_Equal")]
	public static extern void Process_Equal(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000E4E RID: 3662
	[DllImport("SDPCore", EntryPoint = "CSharp_Process_IsValid")]
	public static extern bool Process_IsValid(HandleRef jarg1);

	// Token: 0x06000E4F RID: 3663
	[DllImport("SDPCore", EntryPoint = "CSharp_Process_GetProperties")]
	public static extern IntPtr Process_GetProperties(HandleRef jarg1);

	// Token: 0x06000E50 RID: 3664
	[DllImport("SDPCore", EntryPoint = "CSharp_Process_IsProviderLinked")]
	public static extern bool Process_IsProviderLinked(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E51 RID: 3665
	[DllImport("SDPCore", EntryPoint = "CSharp_Process_GetLinkedProviders")]
	public static extern IntPtr Process_GetLinkedProviders(HandleRef jarg1);

	// Token: 0x06000E52 RID: 3666
	[DllImport("SDPCore", EntryPoint = "CSharp_Process_IsMetricLinked")]
	public static extern bool Process_IsMetricLinked(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E53 RID: 3667
	[DllImport("SDPCore", EntryPoint = "CSharp_Process_GetLinkedMetrics")]
	public static extern IntPtr Process_GetLinkedMetrics(HandleRef jarg1);

	// Token: 0x06000E54 RID: 3668
	[DllImport("SDPCore", EntryPoint = "CSharp_Process_GetIcon")]
	public static extern IntPtr Process_GetIcon(HandleRef jarg1);

	// Token: 0x06000E55 RID: 3669
	[DllImport("SDPCore", EntryPoint = "CSharp_Process_SetIcon")]
	public static extern bool Process_SetIcon(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000E56 RID: 3670
	[DllImport("SDPCore", EntryPoint = "CSharp_Process_SetState")]
	public static extern bool Process_SetState(HandleRef jarg1, int jarg2, long jarg3);

	// Token: 0x06000E57 RID: 3671
	[DllImport("SDPCore", EntryPoint = "CSharp_Process_LinkToProvider")]
	public static extern bool Process_LinkToProvider(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E58 RID: 3672
	[DllImport("SDPCore", EntryPoint = "CSharp_Process_LinkToMetric")]
	public static extern bool Process_LinkToMetric(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E59 RID: 3673
	[DllImport("SDPCore", EntryPoint = "CSharp_Process_AddRealTimeWarning")]
	public static extern void Process_AddRealTimeWarning(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E5A RID: 3674
	[DllImport("SDPCore", EntryPoint = "CSharp_Process_AddTraceWarning")]
	public static extern void Process_AddTraceWarning(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E5B RID: 3675
	[DllImport("SDPCore", EntryPoint = "CSharp_Process_AddSnapshotWarning")]
	public static extern void Process_AddSnapshotWarning(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E5C RID: 3676
	[DllImport("SDPCore", EntryPoint = "CSharp_Process_AddSamplingWarning")]
	public static extern void Process_AddSamplingWarning(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E5D RID: 3677
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryProperties_id_set")]
	public static extern void MetricCategoryProperties_id_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E5E RID: 3678
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryProperties_id_get")]
	public static extern uint MetricCategoryProperties_id_get(HandleRef jarg1);

	// Token: 0x06000E5F RID: 3679
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryProperties_parent_set")]
	public static extern void MetricCategoryProperties_parent_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E60 RID: 3680
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryProperties_parent_get")]
	public static extern uint MetricCategoryProperties_parent_get(HandleRef jarg1);

	// Token: 0x06000E61 RID: 3681
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryProperties_name_set")]
	public static extern void MetricCategoryProperties_name_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000E62 RID: 3682
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryProperties_name_get")]
	public static extern string MetricCategoryProperties_name_get(HandleRef jarg1);

	// Token: 0x06000E63 RID: 3683
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryProperties_description_set")]
	public static extern void MetricCategoryProperties_description_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000E64 RID: 3684
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryProperties_description_get")]
	public static extern string MetricCategoryProperties_description_get(HandleRef jarg1);

	// Token: 0x06000E65 RID: 3685
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryProperties_children_set")]
	public static extern void MetricCategoryProperties_children_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000E66 RID: 3686
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryProperties_children_get")]
	public static extern IntPtr MetricCategoryProperties_children_get(HandleRef jarg1);

	// Token: 0x06000E67 RID: 3687
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryProperties_metrics_set")]
	public static extern void MetricCategoryProperties_metrics_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000E68 RID: 3688
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryProperties_metrics_get")]
	public static extern IntPtr MetricCategoryProperties_metrics_get(HandleRef jarg1);

	// Token: 0x06000E69 RID: 3689
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricCategoryProperties__SWIG_0")]
	public static extern IntPtr new_MetricCategoryProperties__SWIG_0();

	// Token: 0x06000E6A RID: 3690
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricCategoryProperties__SWIG_1")]
	public static extern IntPtr new_MetricCategoryProperties__SWIG_1(HandleRef jarg1);

	// Token: 0x06000E6B RID: 3691
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricCategoryProperties__SWIG_2")]
	public static extern IntPtr new_MetricCategoryProperties__SWIG_2(uint jarg1, string jarg2, string jarg3, uint jarg4);

	// Token: 0x06000E6C RID: 3692
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricCategoryProperties__SWIG_3")]
	public static extern IntPtr new_MetricCategoryProperties__SWIG_3(uint jarg1, string jarg2, string jarg3);

	// Token: 0x06000E6D RID: 3693
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryProperties_Equal")]
	public static extern void MetricCategoryProperties_Equal(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000E6E RID: 3694
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_MetricCategoryProperties")]
	public static extern void delete_MetricCategoryProperties(HandleRef jarg1);

	// Token: 0x06000E6F RID: 3695
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_MetricCategory")]
	public static extern void delete_MetricCategory(HandleRef jarg1);

	// Token: 0x06000E70 RID: 3696
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricCategory__SWIG_0")]
	public static extern IntPtr new_MetricCategory__SWIG_0();

	// Token: 0x06000E71 RID: 3697
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricCategory__SWIG_1")]
	public static extern IntPtr new_MetricCategory__SWIG_1(HandleRef jarg1);

	// Token: 0x06000E72 RID: 3698
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricCategory__SWIG_2")]
	public static extern IntPtr new_MetricCategory__SWIG_2(HandleRef jarg1);

	// Token: 0x06000E73 RID: 3699
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategory_Equal")]
	public static extern void MetricCategory_Equal(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000E74 RID: 3700
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategory_IsValid")]
	public static extern bool MetricCategory_IsValid(HandleRef jarg1);

	// Token: 0x06000E75 RID: 3701
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategory_GetProperties")]
	public static extern IntPtr MetricCategory_GetProperties(HandleRef jarg1);

	// Token: 0x06000E76 RID: 3702
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategory_GetMetrics")]
	public static extern IntPtr MetricCategory_GetMetrics(HandleRef jarg1);

	// Token: 0x06000E77 RID: 3703
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategory_GetParent")]
	public static extern IntPtr MetricCategory_GetParent(HandleRef jarg1);

	// Token: 0x06000E78 RID: 3704
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategory_GetChildren")]
	public static extern IntPtr MetricCategory_GetChildren(HandleRef jarg1);

	// Token: 0x06000E79 RID: 3705
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_AUTOGEN_METRIC_ID_get")]
	public static extern uint SDP_AUTOGEN_METRIC_ID_get();

	// Token: 0x06000E7A RID: 3706
	[DllImport("SDPCore", EntryPoint = "CSharp_CustomDataBuffer_buffer_set")]
	public static extern void CustomDataBuffer_buffer_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000E7B RID: 3707
	[DllImport("SDPCore", EntryPoint = "CSharp_CustomDataBuffer_buffer_get")]
	public static extern IntPtr CustomDataBuffer_buffer_get(HandleRef jarg1);

	// Token: 0x06000E7C RID: 3708
	[DllImport("SDPCore", EntryPoint = "CSharp_new_CustomDataBuffer")]
	public static extern IntPtr new_CustomDataBuffer();

	// Token: 0x06000E7D RID: 3709
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_CustomDataBuffer")]
	public static extern void delete_CustomDataBuffer(HandleRef jarg1);

	// Token: 0x06000E7E RID: 3710
	[DllImport("SDPCore", EntryPoint = "CSharp_kInvalidMetricID_get")]
	public static extern uint kInvalidMetricID_get();

	// Token: 0x06000E7F RID: 3711
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_name_set")]
	public static extern void MetricProperties_name_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000E80 RID: 3712
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_name_get")]
	public static extern string MetricProperties_name_get(HandleRef jarg1);

	// Token: 0x06000E81 RID: 3713
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_description_set")]
	public static extern void MetricProperties_description_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000E82 RID: 3714
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_description_get")]
	public static extern string MetricProperties_description_get(HandleRef jarg1);

	// Token: 0x06000E83 RID: 3715
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_state_set")]
	public static extern void MetricProperties_state_set(HandleRef jarg1, int jarg2);

	// Token: 0x06000E84 RID: 3716
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_state_get")]
	public static extern int MetricProperties_state_get(HandleRef jarg1);

	// Token: 0x06000E85 RID: 3717
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_type_set")]
	public static extern void MetricProperties_type_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E86 RID: 3718
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_type_get")]
	public static extern uint MetricProperties_type_get(HandleRef jarg1);

	// Token: 0x06000E87 RID: 3719
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_global_set")]
	public static extern void MetricProperties_global_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E88 RID: 3720
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_global_get")]
	public static extern uint MetricProperties_global_get(HandleRef jarg1);

	// Token: 0x06000E89 RID: 3721
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_sampleRate_set")]
	public static extern void MetricProperties_sampleRate_set(HandleRef jarg1, float jarg2);

	// Token: 0x06000E8A RID: 3722
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_sampleRate_get")]
	public static extern float MetricProperties_sampleRate_get(HandleRef jarg1);

	// Token: 0x06000E8B RID: 3723
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_captureTypeMask_set")]
	public static extern void MetricProperties_captureTypeMask_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E8C RID: 3724
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_captureTypeMask_get")]
	public static extern uint MetricProperties_captureTypeMask_get(HandleRef jarg1);

	// Token: 0x06000E8D RID: 3725
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_captureState_set")]
	public static extern void MetricProperties_captureState_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E8E RID: 3726
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_captureState_get")]
	public static extern uint MetricProperties_captureState_get(HandleRef jarg1);

	// Token: 0x06000E8F RID: 3727
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_id_set")]
	public static extern void MetricProperties_id_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E90 RID: 3728
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_id_get")]
	public static extern uint MetricProperties_id_get(HandleRef jarg1);

	// Token: 0x06000E91 RID: 3729
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_categoryID_set")]
	public static extern void MetricProperties_categoryID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E92 RID: 3730
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_categoryID_get")]
	public static extern uint MetricProperties_categoryID_get(HandleRef jarg1);

	// Token: 0x06000E93 RID: 3731
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_providerID_set")]
	public static extern void MetricProperties_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E94 RID: 3732
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_providerID_get")]
	public static extern uint MetricProperties_providerID_get(HandleRef jarg1);

	// Token: 0x06000E95 RID: 3733
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_pid_set")]
	public static extern void MetricProperties_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E96 RID: 3734
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_pid_get")]
	public static extern uint MetricProperties_pid_get(HandleRef jarg1);

	// Token: 0x06000E97 RID: 3735
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_lastUpdated_set")]
	public static extern void MetricProperties_lastUpdated_set(HandleRef jarg1, long jarg2);

	// Token: 0x06000E98 RID: 3736
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_lastUpdated_get")]
	public static extern long MetricProperties_lastUpdated_get(HandleRef jarg1);

	// Token: 0x06000E99 RID: 3737
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_hidden_set")]
	public static extern void MetricProperties_hidden_set(HandleRef jarg1, bool jarg2);

	// Token: 0x06000E9A RID: 3738
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_hidden_get")]
	public static extern bool MetricProperties_hidden_get(HandleRef jarg1);

	// Token: 0x06000E9B RID: 3739
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_userData_set")]
	public static extern void MetricProperties_userData_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000E9C RID: 3740
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_userData_get")]
	public static extern uint MetricProperties_userData_get(HandleRef jarg1);

	// Token: 0x06000E9D RID: 3741
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricProperties__SWIG_0")]
	public static extern IntPtr new_MetricProperties__SWIG_0();

	// Token: 0x06000E9E RID: 3742
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricProperties__SWIG_1")]
	public static extern IntPtr new_MetricProperties__SWIG_1(HandleRef jarg1);

	// Token: 0x06000E9F RID: 3743
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricProperties__SWIG_2")]
	public static extern IntPtr new_MetricProperties__SWIG_2(string jarg1, string jarg2, int jarg3, uint jarg4, bool jarg5, float jarg6, uint jarg7, uint jarg8, uint jarg9, bool jarg10, uint jarg11);

	// Token: 0x06000EA0 RID: 3744
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricProperties__SWIG_3")]
	public static extern IntPtr new_MetricProperties__SWIG_3(string jarg1, string jarg2, int jarg3, uint jarg4, bool jarg5, float jarg6, uint jarg7, uint jarg8, uint jarg9, bool jarg10);

	// Token: 0x06000EA1 RID: 3745
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricProperties_Equal")]
	public static extern void MetricProperties_Equal(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000EA2 RID: 3746
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_MetricProperties")]
	public static extern void delete_MetricProperties(HandleRef jarg1);

	// Token: 0x06000EA3 RID: 3747
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_Metric")]
	public static extern void delete_Metric(HandleRef jarg1);

	// Token: 0x06000EA4 RID: 3748
	[DllImport("SDPCore", EntryPoint = "CSharp_new_Metric__SWIG_0")]
	public static extern IntPtr new_Metric__SWIG_0();

	// Token: 0x06000EA5 RID: 3749
	[DllImport("SDPCore", EntryPoint = "CSharp_new_Metric__SWIG_1")]
	public static extern IntPtr new_Metric__SWIG_1(HandleRef jarg1);

	// Token: 0x06000EA6 RID: 3750
	[DllImport("SDPCore", EntryPoint = "CSharp_new_Metric__SWIG_2")]
	public static extern IntPtr new_Metric__SWIG_2(HandleRef jarg1);

	// Token: 0x06000EA7 RID: 3751
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_Equal")]
	public static extern void Metric_Equal(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000EA8 RID: 3752
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_IsValid")]
	public static extern bool Metric_IsValid(HandleRef jarg1);

	// Token: 0x06000EA9 RID: 3753
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_GetProperties")]
	public static extern IntPtr Metric_GetProperties(HandleRef jarg1);

	// Token: 0x06000EAA RID: 3754
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_IsActive")]
	public static extern bool Metric_IsActive(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000EAB RID: 3755
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_IsPaused")]
	public static extern bool Metric_IsPaused(HandleRef jarg1);

	// Token: 0x06000EAC RID: 3756
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_IsGlobal")]
	public static extern bool Metric_IsGlobal(HandleRef jarg1);

	// Token: 0x06000EAD RID: 3757
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_IsRealtimeMetric")]
	public static extern bool Metric_IsRealtimeMetric(HandleRef jarg1);

	// Token: 0x06000EAE RID: 3758
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_IsTraceMetric")]
	public static extern bool Metric_IsTraceMetric(HandleRef jarg1);

	// Token: 0x06000EAF RID: 3759
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_IsSnapshotMetric")]
	public static extern bool Metric_IsSnapshotMetric(HandleRef jarg1);

	// Token: 0x06000EB0 RID: 3760
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_Activate__SWIG_0")]
	public static extern bool Metric_Activate__SWIG_0(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000EB1 RID: 3761
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_Activate__SWIG_1")]
	public static extern bool Metric_Activate__SWIG_1(HandleRef jarg1, uint jarg2);

	// Token: 0x06000EB2 RID: 3762
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_Activate__SWIG_2")]
	public static extern bool Metric_Activate__SWIG_2(HandleRef jarg1);

	// Token: 0x06000EB3 RID: 3763
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_Deactivate__SWIG_0")]
	public static extern bool Metric_Deactivate__SWIG_0(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000EB4 RID: 3764
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_Deactivate__SWIG_1")]
	public static extern bool Metric_Deactivate__SWIG_1(HandleRef jarg1, uint jarg2);

	// Token: 0x06000EB5 RID: 3765
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_Deactivate__SWIG_2")]
	public static extern bool Metric_Deactivate__SWIG_2(HandleRef jarg1);

	// Token: 0x06000EB6 RID: 3766
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_Pause__SWIG_0")]
	public static extern bool Metric_Pause__SWIG_0(HandleRef jarg1, bool jarg2);

	// Token: 0x06000EB7 RID: 3767
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_Pause__SWIG_1")]
	public static extern bool Metric_Pause__SWIG_1(HandleRef jarg1);

	// Token: 0x06000EB8 RID: 3768
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_IsLogReady")]
	public static extern bool Metric_IsLogReady(HandleRef jarg1, long jarg2);

	// Token: 0x06000EB9 RID: 3769
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_Log__SWIG_0")]
	public static extern bool Metric_Log__SWIG_0(HandleRef jarg1, uint jarg2, long jarg3, double jarg4, uint jarg5);

	// Token: 0x06000EBA RID: 3770
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_Log__SWIG_1")]
	public static extern bool Metric_Log__SWIG_1(HandleRef jarg1, uint jarg2, long jarg3, double jarg4);

	// Token: 0x06000EBB RID: 3771
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_Log__SWIG_2")]
	public static extern bool Metric_Log__SWIG_2(HandleRef jarg1, uint jarg2, long jarg3, double jarg4, uint jarg5, uint jarg6);

	// Token: 0x06000EBC RID: 3772
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_GetValue")]
	public static extern double Metric_GetValue(HandleRef jarg1);

	// Token: 0x06000EBD RID: 3773
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_GetActiveProcesses__SWIG_0")]
	public static extern IntPtr Metric_GetActiveProcesses__SWIG_0(HandleRef jarg1, uint jarg2);

	// Token: 0x06000EBE RID: 3774
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_GetActiveProcesses__SWIG_1")]
	public static extern IntPtr Metric_GetActiveProcesses__SWIG_1(HandleRef jarg1);

	// Token: 0x06000EBF RID: 3775
	[DllImport("SDPCore", EntryPoint = "CSharp_Metric_SetHidden")]
	public static extern bool Metric_SetHidden(HandleRef jarg1, bool jarg2);

	// Token: 0x06000EC0 RID: 3776
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_MetricDelegate")]
	public static extern void delete_MetricDelegate(HandleRef jarg1);

	// Token: 0x06000EC1 RID: 3777
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricDelegate_OnMetricAdded")]
	public static extern void MetricDelegate_OnMetricAdded(HandleRef jarg1, uint jarg2);

	// Token: 0x06000EC2 RID: 3778
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricDelegate_OnMetricAddedSwigExplicitMetricDelegate")]
	public static extern void MetricDelegate_OnMetricAddedSwigExplicitMetricDelegate(HandleRef jarg1, uint jarg2);

	// Token: 0x06000EC3 RID: 3779
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricDelegate_OnMetricListReceived")]
	public static extern void MetricDelegate_OnMetricListReceived(HandleRef jarg1, uint jarg2);

	// Token: 0x06000EC4 RID: 3780
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricDelegate_OnMetricListReceivedSwigExplicitMetricDelegate")]
	public static extern void MetricDelegate_OnMetricListReceivedSwigExplicitMetricDelegate(HandleRef jarg1, uint jarg2);

	// Token: 0x06000EC5 RID: 3781
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricDelegate_OnMetricActivated")]
	public static extern void MetricDelegate_OnMetricActivated(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000EC6 RID: 3782
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricDelegate_OnMetricActivatedSwigExplicitMetricDelegate")]
	public static extern void MetricDelegate_OnMetricActivatedSwigExplicitMetricDelegate(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000EC7 RID: 3783
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricDelegate_OnMetricDeactivated")]
	public static extern void MetricDelegate_OnMetricDeactivated(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000EC8 RID: 3784
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricDelegate_OnMetricDeactivatedSwigExplicitMetricDelegate")]
	public static extern void MetricDelegate_OnMetricDeactivatedSwigExplicitMetricDelegate(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000EC9 RID: 3785
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricDelegate_OnMetricHiddenToggled")]
	public static extern void MetricDelegate_OnMetricHiddenToggled(HandleRef jarg1, uint jarg2, bool jarg3);

	// Token: 0x06000ECA RID: 3786
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricDelegate_OnMetricHiddenToggledSwigExplicitMetricDelegate")]
	public static extern void MetricDelegate_OnMetricHiddenToggledSwigExplicitMetricDelegate(HandleRef jarg1, uint jarg2, bool jarg3);

	// Token: 0x06000ECB RID: 3787
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricDelegate_OnMetricDataReceived")]
	public static extern void MetricDelegate_OnMetricDataReceived(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, long jarg5, double jarg6);

	// Token: 0x06000ECC RID: 3788
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricDelegate_OnMetricDataReceivedSwigExplicitMetricDelegate")]
	public static extern void MetricDelegate_OnMetricDataReceivedSwigExplicitMetricDelegate(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, long jarg5, double jarg6);

	// Token: 0x06000ECD RID: 3789
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricDelegate")]
	public static extern IntPtr new_MetricDelegate();

	// Token: 0x06000ECE RID: 3790
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricDelegate_director_connect")]
	public static extern void MetricDelegate_director_connect(HandleRef jarg1, MetricDelegate.SwigDelegateMetricDelegate_0 delegate0, MetricDelegate.SwigDelegateMetricDelegate_1 delegate1, MetricDelegate.SwigDelegateMetricDelegate_2 delegate2, MetricDelegate.SwigDelegateMetricDelegate_3 delegate3, MetricDelegate.SwigDelegateMetricDelegate_4 delegate4, MetricDelegate.SwigDelegateMetricDelegate_5 delegate5);

	// Token: 0x06000ECF RID: 3791
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_MetricCategoryDelegate")]
	public static extern void delete_MetricCategoryDelegate(HandleRef jarg1);

	// Token: 0x06000ED0 RID: 3792
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryDelegate_OnMetricCategoryAdded")]
	public static extern void MetricCategoryDelegate_OnMetricCategoryAdded(HandleRef jarg1, uint jarg2);

	// Token: 0x06000ED1 RID: 3793
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryDelegate_OnMetricCategoryListReceived")]
	public static extern void MetricCategoryDelegate_OnMetricCategoryListReceived(HandleRef jarg1);

	// Token: 0x06000ED2 RID: 3794
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryDelegate_OnMetricCategoryActivated")]
	public static extern void MetricCategoryDelegate_OnMetricCategoryActivated(HandleRef jarg1, uint jarg2);

	// Token: 0x06000ED3 RID: 3795
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricCategoryDelegate")]
	public static extern IntPtr new_MetricCategoryDelegate();

	// Token: 0x06000ED4 RID: 3796
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryDelegate_director_connect")]
	public static extern void MetricCategoryDelegate_director_connect(HandleRef jarg1, MetricCategoryDelegate.SwigDelegateMetricCategoryDelegate_0 delegate0, MetricCategoryDelegate.SwigDelegateMetricCategoryDelegate_1 delegate1, MetricCategoryDelegate.SwigDelegateMetricCategoryDelegate_2 delegate2);

	// Token: 0x06000ED5 RID: 3797
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_MetricManagerDelegate")]
	public static extern void delete_MetricManagerDelegate(HandleRef jarg1);

	// Token: 0x06000ED6 RID: 3798
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManagerDelegate_OnMetricCategoryAdded")]
	public static extern void MetricManagerDelegate_OnMetricCategoryAdded(HandleRef jarg1, uint jarg2);

	// Token: 0x06000ED7 RID: 3799
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManagerDelegate_OnMetricCategoryListReceived")]
	public static extern void MetricManagerDelegate_OnMetricCategoryListReceived(HandleRef jarg1);

	// Token: 0x06000ED8 RID: 3800
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManagerDelegate_OnMetricCategoryActivated")]
	public static extern void MetricManagerDelegate_OnMetricCategoryActivated(HandleRef jarg1, uint jarg2);

	// Token: 0x06000ED9 RID: 3801
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManagerDelegate_OnMetricAdded")]
	public static extern void MetricManagerDelegate_OnMetricAdded(HandleRef jarg1, uint jarg2);

	// Token: 0x06000EDA RID: 3802
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManagerDelegate_OnMetricListReceived")]
	public static extern void MetricManagerDelegate_OnMetricListReceived(HandleRef jarg1, uint jarg2);

	// Token: 0x06000EDB RID: 3803
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManagerDelegate_OnMetricActivated")]
	public static extern void MetricManagerDelegate_OnMetricActivated(HandleRef jarg1, uint jarg2);

	// Token: 0x06000EDC RID: 3804
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManagerDelegate_OnMetricDeactivated")]
	public static extern void MetricManagerDelegate_OnMetricDeactivated(HandleRef jarg1, uint jarg2);

	// Token: 0x06000EDD RID: 3805
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManagerDelegate_OnMetricDataReceived")]
	public static extern void MetricManagerDelegate_OnMetricDataReceived(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, long jarg5, double jarg6);

	// Token: 0x06000EDE RID: 3806
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricManagerDelegate")]
	public static extern IntPtr new_MetricManagerDelegate();

	// Token: 0x06000EDF RID: 3807
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManagerDelegate_director_connect")]
	public static extern void MetricManagerDelegate_director_connect(HandleRef jarg1, MetricManagerDelegate.SwigDelegateMetricManagerDelegate_0 delegate0, MetricManagerDelegate.SwigDelegateMetricManagerDelegate_1 delegate1, MetricManagerDelegate.SwigDelegateMetricManagerDelegate_2 delegate2, MetricManagerDelegate.SwigDelegateMetricManagerDelegate_3 delegate3, MetricManagerDelegate.SwigDelegateMetricManagerDelegate_4 delegate4, MetricManagerDelegate.SwigDelegateMetricManagerDelegate_5 delegate5, MetricManagerDelegate.SwigDelegateMetricManagerDelegate_6 delegate6, MetricManagerDelegate.SwigDelegateMetricManagerDelegate_7 delegate7);

	// Token: 0x06000EE0 RID: 3808
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_SessionEventHandler_OnSessionOpened")]
	public static extern void MetricManager_SessionEventHandler_OnSessionOpened(HandleRef jarg1);

	// Token: 0x06000EE1 RID: 3809
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_SessionEventHandler_OnSessionClosed")]
	public static extern void MetricManager_SessionEventHandler_OnSessionClosed(HandleRef jarg1);

	// Token: 0x06000EE2 RID: 3810
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_MetricManager_SessionEventHandler")]
	public static extern void delete_MetricManager_SessionEventHandler(HandleRef jarg1);

	// Token: 0x06000EE3 RID: 3811
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricManager_SessionEventHandler")]
	public static extern IntPtr new_MetricManager_SessionEventHandler();

	// Token: 0x06000EE4 RID: 3812
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_MetricManager")]
	public static extern void delete_MetricManager(HandleRef jarg1);

	// Token: 0x06000EE5 RID: 3813
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_Get")]
	public static extern IntPtr MetricManager_Get();

	// Token: 0x06000EE6 RID: 3814
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_RegisterMetricEventDelegate")]
	public static extern void MetricManager_RegisterMetricEventDelegate(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000EE7 RID: 3815
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_UnregisterMetricEventDelegate")]
	public static extern void MetricManager_UnregisterMetricEventDelegate(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000EE8 RID: 3816
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_RegisterMetricCategoryEventDelegate")]
	public static extern void MetricManager_RegisterMetricCategoryEventDelegate(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000EE9 RID: 3817
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_UnregisterMetricCategoryEventDelegate")]
	public static extern void MetricManager_UnregisterMetricCategoryEventDelegate(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000EEA RID: 3818
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_Reset")]
	public static extern void MetricManager_Reset(HandleRef jarg1);

	// Token: 0x06000EEB RID: 3819
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_ShutDown")]
	public static extern void MetricManager_ShutDown(HandleRef jarg1);

	// Token: 0x06000EEC RID: 3820
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_DisableDataModel")]
	public static extern void MetricManager_DisableDataModel(HandleRef jarg1);

	// Token: 0x06000EED RID: 3821
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_IsDataModelActive")]
	public static extern bool MetricManager_IsDataModelActive(HandleRef jarg1);

	// Token: 0x06000EEE RID: 3822
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_AddMetricCategory__SWIG_0")]
	public static extern IntPtr MetricManager_AddMetricCategory__SWIG_0(HandleRef jarg1, string jarg2, string jarg3, uint jarg4, bool jarg5);

	// Token: 0x06000EEF RID: 3823
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_AddMetricCategory__SWIG_1")]
	public static extern IntPtr MetricManager_AddMetricCategory__SWIG_1(HandleRef jarg1, string jarg2, string jarg3, uint jarg4);

	// Token: 0x06000EF0 RID: 3824
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_AddMetricCategory__SWIG_2")]
	public static extern IntPtr MetricManager_AddMetricCategory__SWIG_2(HandleRef jarg1, string jarg2, string jarg3);

	// Token: 0x06000EF1 RID: 3825
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_AddMetricCategory__SWIG_3")]
	public static extern IntPtr MetricManager_AddMetricCategory__SWIG_3(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000EF2 RID: 3826
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_GetMetricCategoryByName")]
	public static extern IntPtr MetricManager_GetMetricCategoryByName(HandleRef jarg1, string jarg2);

	// Token: 0x06000EF3 RID: 3827
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_GetMetricCategory")]
	public static extern IntPtr MetricManager_GetMetricCategory(HandleRef jarg1, uint jarg2);

	// Token: 0x06000EF4 RID: 3828
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_GetAllMetricCategories")]
	public static extern IntPtr MetricManager_GetAllMetricCategories(HandleRef jarg1);

	// Token: 0x06000EF5 RID: 3829
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_AddMetric__SWIG_0")]
	public static extern IntPtr MetricManager_AddMetric__SWIG_0(HandleRef jarg1, HandleRef jarg2, bool jarg3);

	// Token: 0x06000EF6 RID: 3830
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_AddMetric__SWIG_1")]
	public static extern IntPtr MetricManager_AddMetric__SWIG_1(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000EF7 RID: 3831
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_GetMetric")]
	public static extern IntPtr MetricManager_GetMetric(HandleRef jarg1, uint jarg2);

	// Token: 0x06000EF8 RID: 3832
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_GetMetricByName")]
	public static extern IntPtr MetricManager_GetMetricByName(HandleRef jarg1, string jarg2);

	// Token: 0x06000EF9 RID: 3833
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_GetAllMetrics")]
	public static extern IntPtr MetricManager_GetAllMetrics(HandleRef jarg1);

	// Token: 0x06000EFA RID: 3834
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_GetMetricsByProvider")]
	public static extern IntPtr MetricManager_GetMetricsByProvider(HandleRef jarg1, uint jarg2);

	// Token: 0x06000EFB RID: 3835
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_LogMetricValue__SWIG_0")]
	public static extern bool MetricManager_LogMetricValue__SWIG_0(HandleRef jarg1, uint jarg2, uint jarg3, long jarg4, double jarg5, uint jarg6);

	// Token: 0x06000EFC RID: 3836
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_LogMetricValue__SWIG_1")]
	public static extern bool MetricManager_LogMetricValue__SWIG_1(HandleRef jarg1, uint jarg2, uint jarg3, long jarg4, double jarg5);

	// Token: 0x06000EFD RID: 3837
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_GetSessionEventHandler")]
	public static extern IntPtr MetricManager_GetSessionEventHandler(HandleRef jarg1);

	// Token: 0x06000EFE RID: 3838
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ClientDelegate")]
	public static extern void delete_ClientDelegate(HandleRef jarg1);

	// Token: 0x06000EFF RID: 3839
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnClientConnected")]
	public static extern void ClientDelegate_OnClientConnected(HandleRef jarg1);

	// Token: 0x06000F00 RID: 3840
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnClientConnectedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnClientConnectedSwigExplicitClientDelegate(HandleRef jarg1);

	// Token: 0x06000F01 RID: 3841
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnClientDisconnected")]
	public static extern void ClientDelegate_OnClientDisconnected(HandleRef jarg1);

	// Token: 0x06000F02 RID: 3842
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnClientDisconnectedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnClientDisconnectedSwigExplicitClientDelegate(HandleRef jarg1);

	// Token: 0x06000F03 RID: 3843
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnCaptureComplete")]
	public static extern void ClientDelegate_OnCaptureComplete(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000F04 RID: 3844
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnCaptureCompleteSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnCaptureCompleteSwigExplicitClientDelegate(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000F05 RID: 3845
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnProviderListReceived")]
	public static extern void ClientDelegate_OnProviderListReceived(HandleRef jarg1);

	// Token: 0x06000F06 RID: 3846
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnProviderListReceivedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnProviderListReceivedSwigExplicitClientDelegate(HandleRef jarg1);

	// Token: 0x06000F07 RID: 3847
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnProcessStateChanged")]
	public static extern void ClientDelegate_OnProcessStateChanged(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F08 RID: 3848
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnProcessStateChangedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnProcessStateChangedSwigExplicitClientDelegate(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F09 RID: 3849
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnProcessAdded")]
	public static extern void ClientDelegate_OnProcessAdded(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F0A RID: 3850
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnProcessAddedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnProcessAddedSwigExplicitClientDelegate(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F0B RID: 3851
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnProcessRemoved")]
	public static extern void ClientDelegate_OnProcessRemoved(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F0C RID: 3852
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnProcessRemovedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnProcessRemovedSwigExplicitClientDelegate(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F0D RID: 3853
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnProcessMetricLinked")]
	public static extern void ClientDelegate_OnProcessMetricLinked(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000F0E RID: 3854
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnProcessMetricLinkedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnProcessMetricLinkedSwigExplicitClientDelegate(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000F0F RID: 3855
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnThreadListReceived")]
	public static extern void ClientDelegate_OnThreadListReceived(HandleRef jarg1);

	// Token: 0x06000F10 RID: 3856
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnThreadListReceivedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnThreadListReceivedSwigExplicitClientDelegate(HandleRef jarg1);

	// Token: 0x06000F11 RID: 3857
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnMetricListReceived")]
	public static extern void ClientDelegate_OnMetricListReceived(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F12 RID: 3858
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnMetricListReceivedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnMetricListReceivedSwigExplicitClientDelegate(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F13 RID: 3859
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnProviderConnected")]
	public static extern void ClientDelegate_OnProviderConnected(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F14 RID: 3860
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnProviderConnectedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnProviderConnectedSwigExplicitClientDelegate(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F15 RID: 3861
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnProviderDisconnected")]
	public static extern void ClientDelegate_OnProviderDisconnected(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

	// Token: 0x06000F16 RID: 3862
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnProviderDisconnectedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnProviderDisconnectedSwigExplicitClientDelegate(HandleRef jarg1, HandleRef jarg2, HandleRef jarg3);

	// Token: 0x06000F17 RID: 3863
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnDeviceListUpdated")]
	public static extern void ClientDelegate_OnDeviceListUpdated(HandleRef jarg1);

	// Token: 0x06000F18 RID: 3864
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnDeviceListUpdatedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnDeviceListUpdatedSwigExplicitClientDelegate(HandleRef jarg1);

	// Token: 0x06000F19 RID: 3865
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnBufferRegistered")]
	public static extern void ClientDelegate_OnBufferRegistered(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5);

	// Token: 0x06000F1A RID: 3866
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnBufferRegisteredSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnBufferRegisteredSwigExplicitClientDelegate(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5);

	// Token: 0x06000F1B RID: 3867
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnOptionAdded")]
	public static extern void ClientDelegate_OnOptionAdded(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000F1C RID: 3868
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnOptionAddedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnOptionAddedSwigExplicitClientDelegate(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000F1D RID: 3869
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnOptionCategoryAdded")]
	public static extern void ClientDelegate_OnOptionCategoryAdded(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000F1E RID: 3870
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnOptionCategoryAddedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnOptionCategoryAddedSwigExplicitClientDelegate(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000F1F RID: 3871
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnMetricAdded")]
	public static extern void ClientDelegate_OnMetricAdded(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000F20 RID: 3872
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnMetricAddedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnMetricAddedSwigExplicitClientDelegate(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000F21 RID: 3873
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnMetricCategoryAdded")]
	public static extern void ClientDelegate_OnMetricCategoryAdded(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000F22 RID: 3874
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnMetricCategoryAddedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnMetricCategoryAddedSwigExplicitClientDelegate(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000F23 RID: 3875
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnPreDataProcessed")]
	public static extern void ClientDelegate_OnPreDataProcessed(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000F24 RID: 3876
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnPreDataProcessedSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnPreDataProcessedSwigExplicitClientDelegate(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000F25 RID: 3877
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnDataProcessed__SWIG_0")]
	public static extern void ClientDelegate_OnDataProcessed__SWIG_0(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, string jarg5);

	// Token: 0x06000F26 RID: 3878
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnDataProcessedSwigExplicitClientDelegate__SWIG_0")]
	public static extern void ClientDelegate_OnDataProcessedSwigExplicitClientDelegate__SWIG_0(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, string jarg5);

	// Token: 0x06000F27 RID: 3879
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnDataProcessed__SWIG_1")]
	public static extern void ClientDelegate_OnDataProcessed__SWIG_1(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000F28 RID: 3880
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnDataProcessedSwigExplicitClientDelegate__SWIG_1")]
	public static extern void ClientDelegate_OnDataProcessedSwigExplicitClientDelegate__SWIG_1(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000F29 RID: 3881
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnBufferTransferProgress")]
	public static extern void ClientDelegate_OnBufferTransferProgress(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6, uint jarg7);

	// Token: 0x06000F2A RID: 3882
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnBufferTransferProgressSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnBufferTransferProgressSwigExplicitClientDelegate(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6, uint jarg7);

	// Token: 0x06000F2B RID: 3883
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnDeviceMemoryLow")]
	public static extern void ClientDelegate_OnDeviceMemoryLow(HandleRef jarg1);

	// Token: 0x06000F2C RID: 3884
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnDeviceMemoryLowSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnDeviceMemoryLowSwigExplicitClientDelegate(HandleRef jarg1);

	// Token: 0x06000F2D RID: 3885
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnMaxCaptureDurationExpired")]
	public static extern void ClientDelegate_OnMaxCaptureDurationExpired(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F2E RID: 3886
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_OnMaxCaptureDurationExpiredSwigExplicitClientDelegate")]
	public static extern void ClientDelegate_OnMaxCaptureDurationExpiredSwigExplicitClientDelegate(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F2F RID: 3887
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ClientDelegate")]
	public static extern IntPtr new_ClientDelegate();

	// Token: 0x06000F30 RID: 3888
	[DllImport("SDPCore", EntryPoint = "CSharp_ClientDelegate_director_connect")]
	public static extern void ClientDelegate_director_connect(HandleRef jarg1, ClientDelegate.SwigDelegateClientDelegate_0 delegate0, ClientDelegate.SwigDelegateClientDelegate_1 delegate1, ClientDelegate.SwigDelegateClientDelegate_2 delegate2, ClientDelegate.SwigDelegateClientDelegate_3 delegate3, ClientDelegate.SwigDelegateClientDelegate_4 delegate4, ClientDelegate.SwigDelegateClientDelegate_5 delegate5, ClientDelegate.SwigDelegateClientDelegate_6 delegate6, ClientDelegate.SwigDelegateClientDelegate_7 delegate7, ClientDelegate.SwigDelegateClientDelegate_8 delegate8, ClientDelegate.SwigDelegateClientDelegate_9 delegate9, ClientDelegate.SwigDelegateClientDelegate_10 delegate10, ClientDelegate.SwigDelegateClientDelegate_11 delegate11, ClientDelegate.SwigDelegateClientDelegate_12 delegate12, ClientDelegate.SwigDelegateClientDelegate_13 delegate13, ClientDelegate.SwigDelegateClientDelegate_14 delegate14, ClientDelegate.SwigDelegateClientDelegate_15 delegate15, ClientDelegate.SwigDelegateClientDelegate_16 delegate16, ClientDelegate.SwigDelegateClientDelegate_17 delegate17, ClientDelegate.SwigDelegateClientDelegate_18 delegate18, ClientDelegate.SwigDelegateClientDelegate_19 delegate19, ClientDelegate.SwigDelegateClientDelegate_20 delegate20, ClientDelegate.SwigDelegateClientDelegate_21 delegate21, ClientDelegate.SwigDelegateClientDelegate_22 delegate22, ClientDelegate.SwigDelegateClientDelegate_23 delegate23);

	// Token: 0x06000F31 RID: 3889
	[DllImport("SDPCore", EntryPoint = "CSharp_new_Client")]
	public static extern IntPtr new_Client();

	// Token: 0x06000F32 RID: 3890
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_Client")]
	public static extern void delete_Client(HandleRef jarg1);

	// Token: 0x06000F33 RID: 3891
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_Init__SWIG_0")]
	public static extern bool Client_Init__SWIG_0(HandleRef jarg1, HandleRef jarg2, string jarg3);

	// Token: 0x06000F34 RID: 3892
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_Init__SWIG_1")]
	public static extern bool Client_Init__SWIG_1(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000F35 RID: 3893
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_Shutdown")]
	public static extern void Client_Shutdown(HandleRef jarg1);

	// Token: 0x06000F36 RID: 3894
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_Update")]
	public static extern bool Client_Update(HandleRef jarg1);

	// Token: 0x06000F37 RID: 3895
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_GetVersionString")]
	public static extern string Client_GetVersionString(HandleRef jarg1);

	// Token: 0x06000F38 RID: 3896
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_GetVersion")]
	public static extern long Client_GetVersion(HandleRef jarg1);

	// Token: 0x06000F39 RID: 3897
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_GetBuildDate")]
	public static extern string Client_GetBuildDate(HandleRef jarg1);

	// Token: 0x06000F3A RID: 3898
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_GetDataModel")]
	public static extern IntPtr Client_GetDataModel(HandleRef jarg1);

	// Token: 0x06000F3B RID: 3899
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_ExportAllMetricData")]
	public static extern bool Client_ExportAllMetricData(HandleRef jarg1, string jarg2, Void_UInt_Fn jarg3);

	// Token: 0x06000F3C RID: 3900
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_GetDeviceManager")]
	public static extern IntPtr Client_GetDeviceManager(HandleRef jarg1);

	// Token: 0x06000F3D RID: 3901
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_GetCaptureManager")]
	public static extern IntPtr Client_GetCaptureManager(HandleRef jarg1);

	// Token: 0x06000F3E RID: 3902
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_PauseRealtimeMetrics__SWIG_0")]
	public static extern bool Client_PauseRealtimeMetrics__SWIG_0(HandleRef jarg1, bool jarg2);

	// Token: 0x06000F3F RID: 3903
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_PauseRealtimeMetrics__SWIG_1")]
	public static extern bool Client_PauseRealtimeMetrics__SWIG_1(HandleRef jarg1);

	// Token: 0x06000F40 RID: 3904
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_GetProcessManager")]
	public static extern IntPtr Client_GetProcessManager(HandleRef jarg1);

	// Token: 0x06000F41 RID: 3905
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_FetchBuffer__SWIG_0")]
	public static extern void Client_FetchBuffer__SWIG_0(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, Void_UInt_UInt_UInt_VoidPtr_UInt_VoidPtr_Fn jarg6, IntPtr jarg7, Void_UInt_UInt_UInt_UInt_UInt_UInt_Fn jarg8, bool jarg9, string jarg10);

	// Token: 0x06000F42 RID: 3906
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_FetchBuffer__SWIG_1")]
	public static extern void Client_FetchBuffer__SWIG_1(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, Void_UInt_UInt_UInt_VoidPtr_UInt_VoidPtr_Fn jarg6, IntPtr jarg7, Void_UInt_UInt_UInt_UInt_UInt_UInt_Fn jarg8, bool jarg9);

	// Token: 0x06000F43 RID: 3907
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_FetchBuffer__SWIG_2")]
	public static extern void Client_FetchBuffer__SWIG_2(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, Void_UInt_UInt_UInt_VoidPtr_UInt_VoidPtr_Fn jarg6, IntPtr jarg7, Void_UInt_UInt_UInt_UInt_UInt_UInt_Fn jarg8);

	// Token: 0x06000F44 RID: 3908
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_FetchBuffer__SWIG_3")]
	public static extern void Client_FetchBuffer__SWIG_3(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, Void_UInt_UInt_UInt_VoidPtr_UInt_VoidPtr_Fn jarg6, IntPtr jarg7);

	// Token: 0x06000F45 RID: 3909
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_FetchBuffer__SWIG_4")]
	public static extern void Client_FetchBuffer__SWIG_4(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, Void_UInt_UInt_UInt_VoidPtr_UInt_VoidPtr_Fn jarg6);

	// Token: 0x06000F46 RID: 3910
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_GetBufferDataSize")]
	public static extern uint Client_GetBufferDataSize(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000F47 RID: 3911
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_GetBufferData")]
	public static extern bool Client_GetBufferData(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, IntPtr jarg5, uint jarg6);

	// Token: 0x06000F48 RID: 3912
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RaisePreDataProcessed")]
	public static extern void Client_RaisePreDataProcessed(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000F49 RID: 3913
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RaiseDataProcessed__SWIG_0")]
	public static extern void Client_RaiseDataProcessed__SWIG_0(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, string jarg5);

	// Token: 0x06000F4A RID: 3914
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RaiseDataProcessed__SWIG_1")]
	public static extern void Client_RaiseDataProcessed__SWIG_1(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x06000F4B RID: 3915
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RaiseBufferTransferProgress")]
	public static extern void Client_RaiseBufferTransferProgress(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6, uint jarg7);

	// Token: 0x06000F4C RID: 3916
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RaiseMaxCaptureDurationExpired")]
	public static extern void Client_RaiseMaxCaptureDurationExpired(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F4D RID: 3917
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_PushFile")]
	public static extern bool Client_PushFile(HandleRef jarg1, string jarg2, string jarg3, bool jarg4);

	// Token: 0x06000F4E RID: 3918
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_GetDataProviders")]
	public static extern IntPtr Client_GetDataProviders(HandleRef jarg1);

	// Token: 0x06000F4F RID: 3919
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_GetDeviceList")]
	public static extern IntPtr Client_GetDeviceList(HandleRef jarg1);

	// Token: 0x06000F50 RID: 3920
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_GetProvider")]
	public static extern IntPtr Client_GetProvider(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F51 RID: 3921
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_GetOption__SWIG_0")]
	public static extern IntPtr Client_GetOption__SWIG_0(HandleRef jarg1, string jarg2, uint jarg3);

	// Token: 0x06000F52 RID: 3922
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_GetOption__SWIG_1")]
	public static extern IntPtr Client_GetOption__SWIG_1(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000F53 RID: 3923
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_GetOptionCategory")]
	public static extern IntPtr Client_GetOptionCategory(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F54 RID: 3924
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RequestOptionCategories__SWIG_0")]
	public static extern bool Client_RequestOptionCategories__SWIG_0(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F55 RID: 3925
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RequestOptionCategories__SWIG_1")]
	public static extern bool Client_RequestOptionCategories__SWIG_1(HandleRef jarg1);

	// Token: 0x06000F56 RID: 3926
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RequestOptions__SWIG_0")]
	public static extern bool Client_RequestOptions__SWIG_0(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F57 RID: 3927
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RequestOptions__SWIG_1")]
	public static extern bool Client_RequestOptions__SWIG_1(HandleRef jarg1);

	// Token: 0x06000F58 RID: 3928
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RequestMetricCategories__SWIG_0")]
	public static extern bool Client_RequestMetricCategories__SWIG_0(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F59 RID: 3929
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RequestMetricCategories__SWIG_1")]
	public static extern bool Client_RequestMetricCategories__SWIG_1(HandleRef jarg1);

	// Token: 0x06000F5A RID: 3930
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RequestMetrics__SWIG_0")]
	public static extern bool Client_RequestMetrics__SWIG_0(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F5B RID: 3931
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RequestMetrics__SWIG_1")]
	public static extern bool Client_RequestMetrics__SWIG_1(HandleRef jarg1);

	// Token: 0x06000F5C RID: 3932
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RegisterEventDelegate")]
	public static extern void Client_RegisterEventDelegate(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000F5D RID: 3933
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_DeregisterEventDelegate")]
	public static extern void Client_DeregisterEventDelegate(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000F5E RID: 3934
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RegisterOnBufferRegistered__SWIG_0")]
	public static extern void Client_RegisterOnBufferRegistered__SWIG_0(HandleRef jarg1, Void_UInt_UInt_UInt_UInt_Fn jarg2);

	// Token: 0x06000F5F RID: 3935
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RegisterOnBufferRegistered__SWIG_1")]
	public static extern void Client_RegisterOnBufferRegistered__SWIG_1(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000F60 RID: 3936
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_RemoveProcessOptions")]
	public static extern void Client_RemoveProcessOptions(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F61 RID: 3937
	[DllImport("SDPCore", EntryPoint = "CSharp_new_DataProvider__SWIG_0")]
	public static extern IntPtr new_DataProvider__SWIG_0(HandleRef jarg1, bool jarg2);

	// Token: 0x06000F62 RID: 3938
	[DllImport("SDPCore", EntryPoint = "CSharp_new_DataProvider__SWIG_1")]
	public static extern IntPtr new_DataProvider__SWIG_1(string jarg1, string jarg2, bool jarg3, bool jarg4);

	// Token: 0x06000F63 RID: 3939
	[DllImport("SDPCore", EntryPoint = "CSharp_new_DataProvider__SWIG_2")]
	public static extern IntPtr new_DataProvider__SWIG_2(string jarg1, string jarg2, bool jarg3);

	// Token: 0x06000F64 RID: 3940
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_DataProvider")]
	public static extern void delete_DataProvider(HandleRef jarg1);

	// Token: 0x06000F65 RID: 3941
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_CreateDataProvider__SWIG_0")]
	public static extern IntPtr DataProvider_CreateDataProvider__SWIG_0(string jarg1, string jarg2, bool jarg3);

	// Token: 0x06000F66 RID: 3942
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_CreateDataProvider__SWIG_1")]
	public static extern IntPtr DataProvider_CreateDataProvider__SWIG_1(string jarg1, string jarg2);

	// Token: 0x06000F67 RID: 3943
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_CreateClientDataProvider__SWIG_0")]
	public static extern IntPtr DataProvider_CreateClientDataProvider__SWIG_0(string jarg1, string jarg2, bool jarg3);

	// Token: 0x06000F68 RID: 3944
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_CreateClientDataProvider__SWIG_1")]
	public static extern IntPtr DataProvider_CreateClientDataProvider__SWIG_1(string jarg1, string jarg2);

	// Token: 0x06000F69 RID: 3945
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_DestroyDataProvider")]
	public static extern void DataProvider_DestroyDataProvider(HandleRef jarg1);

	// Token: 0x06000F6A RID: 3946
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_SetID")]
	public static extern void DataProvider_SetID(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F6B RID: 3947
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_Connect__SWIG_0")]
	public static extern bool DataProvider_Connect__SWIG_0(HandleRef jarg1, bool jarg2);

	// Token: 0x06000F6C RID: 3948
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_Connect__SWIG_1")]
	public static extern bool DataProvider_Connect__SWIG_1(HandleRef jarg1);

	// Token: 0x06000F6D RID: 3949
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_Update")]
	public static extern bool DataProvider_Update(HandleRef jarg1);

	// Token: 0x06000F6E RID: 3950
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_WaitForCommand")]
	public static extern bool DataProvider_WaitForCommand(HandleRef jarg1);

	// Token: 0x06000F6F RID: 3951
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_GetProviderDesc")]
	public static extern IntPtr DataProvider_GetProviderDesc(HandleRef jarg1);

	// Token: 0x06000F70 RID: 3952
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_ReportCaptureComplete")]
	public static extern bool DataProvider_ReportCaptureComplete(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F71 RID: 3953
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_RegisterBuffer__SWIG_0")]
	public static extern void DataProvider_RegisterBuffer__SWIG_0(HandleRef jarg1, uint jarg2, HandleRef jarg3, IntPtr jarg4, uint jarg5);

	// Token: 0x06000F72 RID: 3954
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_RegisterBuffer__SWIG_1")]
	public static extern void DataProvider_RegisterBuffer__SWIG_1(HandleRef jarg1, uint jarg2, HandleRef jarg3, HandleRef jarg4, uint jarg5);

	// Token: 0x06000F73 RID: 3955
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_UnregisterBuffer")]
	public static extern void DataProvider_UnregisterBuffer(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000F74 RID: 3956
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_OnBinaryOptionDataReceived")]
	public static extern void DataProvider_OnBinaryOptionDataReceived(HandleRef jarg1, HandleRef jarg2, uint jarg3, uint jarg4, uint jarg5, IntPtr jarg6, uint jarg7);

	// Token: 0x06000F75 RID: 3957
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddOptionCategory__SWIG_0")]
	public static extern IntPtr DataProvider_AddOptionCategory__SWIG_0(HandleRef jarg1, string jarg2, string jarg3, HandleRef jarg4);

	// Token: 0x06000F76 RID: 3958
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddOptionCategory__SWIG_1")]
	public static extern IntPtr DataProvider_AddOptionCategory__SWIG_1(HandleRef jarg1, string jarg2, string jarg3);

	// Token: 0x06000F77 RID: 3959
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddOptionCategory__SWIG_2")]
	public static extern IntPtr DataProvider_AddOptionCategory__SWIG_2(HandleRef jarg1, string jarg2);

	// Token: 0x06000F78 RID: 3960
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_GetOptionCategory__SWIG_0")]
	public static extern IntPtr DataProvider_GetOptionCategory__SWIG_0(HandleRef jarg1, string jarg2);

	// Token: 0x06000F79 RID: 3961
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_GetOptionCategory__SWIG_1")]
	public static extern IntPtr DataProvider_GetOptionCategory__SWIG_1(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F7A RID: 3962
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddOption__SWIG_0")]
	public static extern IntPtr DataProvider_AddOption__SWIG_0(HandleRef jarg1, string jarg2, int jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8);

	// Token: 0x06000F7B RID: 3963
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddOption__SWIG_1")]
	public static extern IntPtr DataProvider_AddOption__SWIG_1(HandleRef jarg1, string jarg2, int jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7);

	// Token: 0x06000F7C RID: 3964
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_GetOption__SWIG_0")]
	public static extern IntPtr DataProvider_GetOption__SWIG_0(HandleRef jarg1, string jarg2, uint jarg3);

	// Token: 0x06000F7D RID: 3965
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_GetOption__SWIG_1")]
	public static extern IntPtr DataProvider_GetOption__SWIG_1(HandleRef jarg1, uint jarg2, uint jarg3);

	// Token: 0x06000F7E RID: 3966
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_RemoveProcessOptions")]
	public static extern void DataProvider_RemoveProcessOptions(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F7F RID: 3967
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddProcess__SWIG_0")]
	public static extern void DataProvider_AddProcess__SWIG_0(HandleRef jarg1, uint jarg2, string jarg3, string jarg4, uint jarg5, uint jarg6, uint jarg7, uint jarg8);

	// Token: 0x06000F80 RID: 3968
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddProcess__SWIG_1")]
	public static extern void DataProvider_AddProcess__SWIG_1(HandleRef jarg1, uint jarg2, string jarg3, string jarg4, uint jarg5, uint jarg6, uint jarg7);

	// Token: 0x06000F81 RID: 3969
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddProcess__SWIG_2")]
	public static extern void DataProvider_AddProcess__SWIG_2(HandleRef jarg1, uint jarg2, string jarg3, string jarg4, uint jarg5, uint jarg6);

	// Token: 0x06000F82 RID: 3970
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddProcess__SWIG_3")]
	public static extern void DataProvider_AddProcess__SWIG_3(HandleRef jarg1, uint jarg2, string jarg3, string jarg4, uint jarg5);

	// Token: 0x06000F83 RID: 3971
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddProcess__SWIG_4")]
	public static extern void DataProvider_AddProcess__SWIG_4(HandleRef jarg1, uint jarg2, string jarg3, string jarg4);

	// Token: 0x06000F84 RID: 3972
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddProcess__SWIG_5")]
	public static extern void DataProvider_AddProcess__SWIG_5(HandleRef jarg1, uint jarg2, string jarg3);

	// Token: 0x06000F85 RID: 3973
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddProcess__SWIG_6")]
	public static extern void DataProvider_AddProcess__SWIG_6(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F86 RID: 3974
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_RemoveProcess")]
	public static extern void DataProvider_RemoveProcess(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F87 RID: 3975
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_IsProcessTracked")]
	public static extern bool DataProvider_IsProcessTracked(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F88 RID: 3976
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_IsProcessSelected")]
	public static extern bool DataProvider_IsProcessSelected(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F89 RID: 3977
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_GetProcessName")]
	public static extern string DataProvider_GetProcessName(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F8A RID: 3978
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_GetProcessUid")]
	public static extern uint DataProvider_GetProcessUid(HandleRef jarg1, uint jarg2);

	// Token: 0x06000F8B RID: 3979
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_GetProcessList")]
	public static extern IntPtr DataProvider_GetProcessList(HandleRef jarg1);

	// Token: 0x06000F8C RID: 3980
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddMetric__SWIG_0")]
	public static extern IntPtr DataProvider_AddMetric__SWIG_0(HandleRef jarg1, string jarg2, int jarg3, uint jarg4, bool jarg5, float jarg6, uint jarg7, string jarg8, uint jarg9, bool jarg10, uint jarg11);

	// Token: 0x06000F8D RID: 3981
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddMetric__SWIG_1")]
	public static extern IntPtr DataProvider_AddMetric__SWIG_1(HandleRef jarg1, string jarg2, int jarg3, uint jarg4, bool jarg5, float jarg6, uint jarg7, string jarg8, uint jarg9, bool jarg10);

	// Token: 0x06000F8E RID: 3982
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddMetric__SWIG_2")]
	public static extern IntPtr DataProvider_AddMetric__SWIG_2(HandleRef jarg1, string jarg2, int jarg3, uint jarg4, bool jarg5, float jarg6, uint jarg7, string jarg8, uint jarg9);

	// Token: 0x06000F8F RID: 3983
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddMetric__SWIG_3")]
	public static extern IntPtr DataProvider_AddMetric__SWIG_3(HandleRef jarg1, string jarg2, int jarg3, uint jarg4, bool jarg5, float jarg6, uint jarg7, string jarg8);

	// Token: 0x06000F90 RID: 3984
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddMetric__SWIG_4")]
	public static extern IntPtr DataProvider_AddMetric__SWIG_4(HandleRef jarg1, string jarg2, int jarg3, uint jarg4, bool jarg5, float jarg6, uint jarg7);

	// Token: 0x06000F91 RID: 3985
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddMetricCategory__SWIG_0")]
	public static extern IntPtr DataProvider_AddMetricCategory__SWIG_0(HandleRef jarg1, string jarg2, string jarg3, uint jarg4);

	// Token: 0x06000F92 RID: 3986
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_AddMetricCategory__SWIG_1")]
	public static extern IntPtr DataProvider_AddMetricCategory__SWIG_1(HandleRef jarg1, string jarg2, string jarg3);

	// Token: 0x06000F93 RID: 3987
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_RegisterOnStartCapture__SWIG_0")]
	public static extern void DataProvider_RegisterOnStartCapture__SWIG_0(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000F94 RID: 3988
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_RegisterOnStopCapture__SWIG_0")]
	public static extern void DataProvider_RegisterOnStopCapture__SWIG_0(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000F95 RID: 3989
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_RegisterOnCancelCapture__SWIG_0")]
	public static extern void DataProvider_RegisterOnCancelCapture__SWIG_0(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000F96 RID: 3990
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_RegisterOnDataProviderConnected")]
	public static extern void DataProvider_RegisterOnDataProviderConnected(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000F97 RID: 3991
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_RegisterOnDataProviderDisconnected")]
	public static extern void DataProvider_RegisterOnDataProviderDisconnected(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000F98 RID: 3992
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_RegisterOnStartCapture__SWIG_1")]
	public static extern void DataProvider_RegisterOnStartCapture__SWIG_1(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000F99 RID: 3993
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_RegisterOnStopCapture__SWIG_1")]
	public static extern void DataProvider_RegisterOnStopCapture__SWIG_1(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000F9A RID: 3994
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_RegisterOnCancelCapture__SWIG_1")]
	public static extern void DataProvider_RegisterOnCancelCapture__SWIG_1(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000F9B RID: 3995
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_RegisterOnMetricToggled")]
	public static extern void DataProvider_RegisterOnMetricToggled(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000F9C RID: 3996
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_GetRealtimeCaptureID")]
	public static extern uint DataProvider_GetRealtimeCaptureID(HandleRef jarg1);

	// Token: 0x06000F9D RID: 3997
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_FindPackageName")]
	public static extern string DataProvider_FindPackageName(HandleRef jarg1, string jarg2, HandleRef jarg3);

	// Token: 0x06000F9E RID: 3998
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureStream_GetProcessID")]
	public static extern uint CaptureStream_GetProcessID(HandleRef jarg1);

	// Token: 0x06000F9F RID: 3999
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureStream_GetThreadID")]
	public static extern uint CaptureStream_GetThreadID(HandleRef jarg1);

	// Token: 0x06000FA0 RID: 4000
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_CaptureStream")]
	public static extern void delete_CaptureStream(HandleRef jarg1);

	// Token: 0x06000FA1 RID: 4001
	[DllImport("SDPCore", EntryPoint = "CSharp_new_CaptureStreamKey")]
	public static extern IntPtr new_CaptureStreamKey(uint jarg1, uint jarg2);

	// Token: 0x06000FA2 RID: 4002
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureStreamKey_LessThan")]
	public static extern bool CaptureStreamKey_LessThan(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000FA3 RID: 4003
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureStreamKey_m_process_set")]
	public static extern void CaptureStreamKey_m_process_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FA4 RID: 4004
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureStreamKey_m_process_get")]
	public static extern uint CaptureStreamKey_m_process_get(HandleRef jarg1);

	// Token: 0x06000FA5 RID: 4005
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureStreamKey_m_thread_set")]
	public static extern void CaptureStreamKey_m_thread_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FA6 RID: 4006
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureStreamKey_m_thread_get")]
	public static extern uint CaptureStreamKey_m_thread_get(HandleRef jarg1);

	// Token: 0x06000FA7 RID: 4007
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_CaptureStreamKey")]
	public static extern void delete_CaptureStreamKey(HandleRef jarg1);

	// Token: 0x06000FA8 RID: 4008
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_m_ID_set")]
	public static extern void ProviderDesc_m_ID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FA9 RID: 4009
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_m_ID_get")]
	public static extern uint ProviderDesc_m_ID_get(HandleRef jarg1);

	// Token: 0x06000FAA RID: 4010
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_filePort_set")]
	public static extern void ProviderDesc_filePort_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FAB RID: 4011
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_filePort_get")]
	public static extern uint ProviderDesc_filePort_get(HandleRef jarg1);

	// Token: 0x06000FAC RID: 4012
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_optionPort_set")]
	public static extern void ProviderDesc_optionPort_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FAD RID: 4013
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_optionPort_get")]
	public static extern uint ProviderDesc_optionPort_get(HandleRef jarg1);

	// Token: 0x06000FAE RID: 4014
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_isClient_set")]
	public static extern void ProviderDesc_isClient_set(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06000FAF RID: 4015
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_isClient_get")]
	public static extern IntPtr ProviderDesc_isClient_get(HandleRef jarg1);

	// Token: 0x06000FB0 RID: 4016
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_Name_set")]
	public static extern void ProviderDesc_Name_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000FB1 RID: 4017
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_Name_get")]
	public static extern string ProviderDesc_Name_get(HandleRef jarg1);

	// Token: 0x06000FB2 RID: 4018
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_Description_set")]
	public static extern void ProviderDesc_Description_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000FB3 RID: 4019
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_Description_get")]
	public static extern string ProviderDesc_Description_get(HandleRef jarg1);

	// Token: 0x06000FB4 RID: 4020
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_ProcessID_set")]
	public static extern void ProviderDesc_ProcessID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FB5 RID: 4021
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_ProcessID_get")]
	public static extern uint ProviderDesc_ProcessID_get(HandleRef jarg1);

	// Token: 0x06000FB6 RID: 4022
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_ProcessName_set")]
	public static extern void ProviderDesc_ProcessName_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000FB7 RID: 4023
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_ProcessName_get")]
	public static extern string ProviderDesc_ProcessName_get(HandleRef jarg1);

	// Token: 0x06000FB8 RID: 4024
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_ThreadID_set")]
	public static extern void ProviderDesc_ThreadID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FB9 RID: 4025
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_ThreadID_get")]
	public static extern uint ProviderDesc_ThreadID_get(HandleRef jarg1);

	// Token: 0x06000FBA RID: 4026
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_ThreadName_set")]
	public static extern void ProviderDesc_ThreadName_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000FBB RID: 4027
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_ThreadName_get")]
	public static extern string ProviderDesc_ThreadName_get(HandleRef jarg1);

	// Token: 0x06000FBC RID: 4028
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_DataHandlerPluginID_set")]
	public static extern void ProviderDesc_DataHandlerPluginID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FBD RID: 4029
	[DllImport("SDPCore", EntryPoint = "CSharp_ProviderDesc_DataHandlerPluginID_get")]
	public static extern uint ProviderDesc_DataHandlerPluginID_get(HandleRef jarg1);

	// Token: 0x06000FBE RID: 4030
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ProviderDesc")]
	public static extern void delete_ProviderDesc(HandleRef jarg1);

	// Token: 0x06000FBF RID: 4031
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestOptions_providerID_set")]
	public static extern void RequestOptions_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FC0 RID: 4032
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestOptions_providerID_get")]
	public static extern uint RequestOptions_providerID_get(HandleRef jarg1);

	// Token: 0x06000FC1 RID: 4033
	[DllImport("SDPCore", EntryPoint = "CSharp_new_RequestOptions")]
	public static extern IntPtr new_RequestOptions(uint jarg1);

	// Token: 0x06000FC2 RID: 4034
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_RequestOptions")]
	public static extern void delete_RequestOptions(HandleRef jarg1);

	// Token: 0x06000FC3 RID: 4035
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_providerID_set")]
	public static extern void ReplyOption_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FC4 RID: 4036
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_providerID_get")]
	public static extern uint ReplyOption_providerID_get(HandleRef jarg1);

	// Token: 0x06000FC5 RID: 4037
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_id_set")]
	public static extern void ReplyOption_id_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FC6 RID: 4038
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_id_get")]
	public static extern uint ReplyOption_id_get(HandleRef jarg1);

	// Token: 0x06000FC7 RID: 4039
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_dataType_set")]
	public static extern void ReplyOption_dataType_set(HandleRef jarg1, int jarg2);

	// Token: 0x06000FC8 RID: 4040
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_dataType_get")]
	public static extern int ReplyOption_dataType_get(HandleRef jarg1);

	// Token: 0x06000FC9 RID: 4041
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_attributes_set")]
	public static extern void ReplyOption_attributes_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FCA RID: 4042
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_attributes_get")]
	public static extern uint ReplyOption_attributes_get(HandleRef jarg1);

	// Token: 0x06000FCB RID: 4043
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_categoryID_set")]
	public static extern void ReplyOption_categoryID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FCC RID: 4044
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_categoryID_get")]
	public static extern uint ReplyOption_categoryID_get(HandleRef jarg1);

	// Token: 0x06000FCD RID: 4045
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_pid_set")]
	public static extern void ReplyOption_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FCE RID: 4046
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_pid_get")]
	public static extern uint ReplyOption_pid_get(HandleRef jarg1);

	// Token: 0x06000FCF RID: 4047
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_name_set")]
	public static extern void ReplyOption_name_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000FD0 RID: 4048
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_name_get")]
	public static extern string ReplyOption_name_get(HandleRef jarg1);

	// Token: 0x06000FD1 RID: 4049
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_description_set")]
	public static extern void ReplyOption_description_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000FD2 RID: 4050
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_description_get")]
	public static extern string ReplyOption_description_get(HandleRef jarg1);

	// Token: 0x06000FD3 RID: 4051
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_initialValue_set")]
	public static extern void ReplyOption_initialValue_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000FD4 RID: 4052
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_initialValue_get")]
	public static extern string ReplyOption_initialValue_get(HandleRef jarg1);

	// Token: 0x06000FD5 RID: 4053
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyOption")]
	public static extern IntPtr new_ReplyOption(uint jarg1, uint jarg2, int jarg3, string jarg4, string jarg5, string jarg6, uint jarg7, uint jarg8, uint jarg9);

	// Token: 0x06000FD6 RID: 4054
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReplyOption")]
	public static extern void delete_ReplyOption(HandleRef jarg1);

	// Token: 0x06000FD7 RID: 4055
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionSet_id_set")]
	public static extern void ReplyOptionSet_id_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FD8 RID: 4056
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionSet_id_get")]
	public static extern uint ReplyOptionSet_id_get(HandleRef jarg1);

	// Token: 0x06000FD9 RID: 4057
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionSet_pid_set")]
	public static extern void ReplyOptionSet_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FDA RID: 4058
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionSet_pid_get")]
	public static extern uint ReplyOptionSet_pid_get(HandleRef jarg1);

	// Token: 0x06000FDB RID: 4059
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionSet_value_set")]
	public static extern void ReplyOptionSet_value_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000FDC RID: 4060
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionSet_value_get")]
	public static extern string ReplyOptionSet_value_get(HandleRef jarg1);

	// Token: 0x06000FDD RID: 4061
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyOptionSet")]
	public static extern IntPtr new_ReplyOptionSet(uint jarg1, uint jarg2, string jarg3);

	// Token: 0x06000FDE RID: 4062
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReplyOptionSet")]
	public static extern void delete_ReplyOptionSet(HandleRef jarg1);

	// Token: 0x06000FDF RID: 4063
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestOptionReset_id_set")]
	public static extern void RequestOptionReset_id_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FE0 RID: 4064
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestOptionReset_id_get")]
	public static extern uint RequestOptionReset_id_get(HandleRef jarg1);

	// Token: 0x06000FE1 RID: 4065
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestOptionReset_pid_set")]
	public static extern void RequestOptionReset_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FE2 RID: 4066
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestOptionReset_pid_get")]
	public static extern uint RequestOptionReset_pid_get(HandleRef jarg1);

	// Token: 0x06000FE3 RID: 4067
	[DllImport("SDPCore", EntryPoint = "CSharp_new_RequestOptionReset")]
	public static extern IntPtr new_RequestOptionReset(uint jarg1, uint jarg2);

	// Token: 0x06000FE4 RID: 4068
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_RequestOptionReset")]
	public static extern void delete_RequestOptionReset(HandleRef jarg1);

	// Token: 0x06000FE5 RID: 4069
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionAttribute_providerID_set")]
	public static extern void ReplyOptionAttribute_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FE6 RID: 4070
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionAttribute_providerID_get")]
	public static extern uint ReplyOptionAttribute_providerID_get(HandleRef jarg1);

	// Token: 0x06000FE7 RID: 4071
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionAttribute_id_set")]
	public static extern void ReplyOptionAttribute_id_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FE8 RID: 4072
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionAttribute_id_get")]
	public static extern uint ReplyOptionAttribute_id_get(HandleRef jarg1);

	// Token: 0x06000FE9 RID: 4073
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionAttribute_pid_set")]
	public static extern void ReplyOptionAttribute_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FEA RID: 4074
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionAttribute_pid_get")]
	public static extern uint ReplyOptionAttribute_pid_get(HandleRef jarg1);

	// Token: 0x06000FEB RID: 4075
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionAttribute_attrIdx_set")]
	public static extern void ReplyOptionAttribute_attrIdx_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FEC RID: 4076
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionAttribute_attrIdx_get")]
	public static extern uint ReplyOptionAttribute_attrIdx_get(HandleRef jarg1);

	// Token: 0x06000FED RID: 4077
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionAttribute_offset_set")]
	public static extern void ReplyOptionAttribute_offset_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FEE RID: 4078
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionAttribute_offset_get")]
	public static extern uint ReplyOptionAttribute_offset_get(HandleRef jarg1);

	// Token: 0x06000FEF RID: 4079
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionAttribute_type_set")]
	public static extern void ReplyOptionAttribute_type_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06000FF0 RID: 4080
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionAttribute_type_get")]
	public static extern uint ReplyOptionAttribute_type_get(HandleRef jarg1);

	// Token: 0x06000FF1 RID: 4081
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionAttribute_strName_set")]
	public static extern void ReplyOptionAttribute_strName_set(HandleRef jarg1, string jarg2);

	// Token: 0x06000FF2 RID: 4082
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionAttribute_strName_get")]
	public static extern string ReplyOptionAttribute_strName_get(HandleRef jarg1);

	// Token: 0x06000FF3 RID: 4083
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyOptionAttribute")]
	public static extern IntPtr new_ReplyOptionAttribute(uint jarg1, uint jarg2, uint jarg3, uint jarg4, uint jarg5, uint jarg6, string jarg7);

	// Token: 0x06000FF4 RID: 4084
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReplyOptionAttribute")]
	public static extern void delete_ReplyOptionAttribute(HandleRef jarg1);

	// Token: 0x06000FF5 RID: 4085
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetID")]
	public static extern uint Option_GetID(HandleRef jarg1);

	// Token: 0x06000FF6 RID: 4086
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetName")]
	public static extern string Option_GetName(HandleRef jarg1);

	// Token: 0x06000FF7 RID: 4087
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetDescription")]
	public static extern string Option_GetDescription(HandleRef jarg1);

	// Token: 0x06000FF8 RID: 4088
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetOptionType")]
	public static extern int Option_GetOptionType(HandleRef jarg1);

	// Token: 0x06000FF9 RID: 4089
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetDataProviderId")]
	public static extern uint Option_GetDataProviderId(HandleRef jarg1);

	// Token: 0x06000FFA RID: 4090
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetCategory")]
	public static extern IntPtr Option_GetCategory(HandleRef jarg1);

	// Token: 0x06000FFB RID: 4091
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetProcessId")]
	public static extern uint Option_GetProcessId(HandleRef jarg1);

	// Token: 0x06000FFC RID: 4092
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_IsOptionHidden")]
	public static extern bool Option_IsOptionHidden(HandleRef jarg1);

	// Token: 0x06000FFD RID: 4093
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_IsOptionReadonly")]
	public static extern bool Option_IsOptionReadonly(HandleRef jarg1);

	// Token: 0x06000FFE RID: 4094
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_IsOptionContextState")]
	public static extern bool Option_IsOptionContextState(HandleRef jarg1);

	// Token: 0x06000FFF RID: 4095
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_IsOptionProcInfo")]
	public static extern bool Option_IsOptionProcInfo(HandleRef jarg1);

	// Token: 0x06001000 RID: 4096
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_IsOptionLaunchApplication")]
	public static extern bool Option_IsOptionLaunchApplication(HandleRef jarg1);

	// Token: 0x06001001 RID: 4097
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetAttributes")]
	public static extern bool Option_GetAttributes(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06001002 RID: 4098
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetAttributes")]
	public static extern bool Option_SetAttributes(HandleRef jarg1, uint jarg2);

	// Token: 0x06001003 RID: 4099
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_0")]
	public static extern bool Option_SetValue__SWIG_0(HandleRef jarg1, bool jarg2, bool jarg3);

	// Token: 0x06001004 RID: 4100
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_1")]
	public static extern bool Option_SetValue__SWIG_1(HandleRef jarg1, bool jarg2);

	// Token: 0x06001005 RID: 4101
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_2")]
	public static extern bool Option_SetValue__SWIG_2(HandleRef jarg1, int jarg2, bool jarg3);

	// Token: 0x06001006 RID: 4102
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_3")]
	public static extern bool Option_SetValue__SWIG_3(HandleRef jarg1, int jarg2);

	// Token: 0x06001007 RID: 4103
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_4")]
	public static extern bool Option_SetValue__SWIG_4(HandleRef jarg1, long jarg2, bool jarg3);

	// Token: 0x06001008 RID: 4104
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_5")]
	public static extern bool Option_SetValue__SWIG_5(HandleRef jarg1, long jarg2);

	// Token: 0x06001009 RID: 4105
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_6")]
	public static extern bool Option_SetValue__SWIG_6(HandleRef jarg1, uint jarg2, bool jarg3);

	// Token: 0x0600100A RID: 4106
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_7")]
	public static extern bool Option_SetValue__SWIG_7(HandleRef jarg1, uint jarg2);

	// Token: 0x0600100B RID: 4107
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_8")]
	public static extern bool Option_SetValue__SWIG_8(HandleRef jarg1, ulong jarg2, bool jarg3);

	// Token: 0x0600100C RID: 4108
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_9")]
	public static extern bool Option_SetValue__SWIG_9(HandleRef jarg1, ulong jarg2);

	// Token: 0x0600100D RID: 4109
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_10")]
	public static extern bool Option_SetValue__SWIG_10(HandleRef jarg1, float jarg2, bool jarg3);

	// Token: 0x0600100E RID: 4110
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_11")]
	public static extern bool Option_SetValue__SWIG_11(HandleRef jarg1, float jarg2);

	// Token: 0x0600100F RID: 4111
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_12")]
	public static extern bool Option_SetValue__SWIG_12(HandleRef jarg1, double jarg2, bool jarg3);

	// Token: 0x06001010 RID: 4112
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_13")]
	public static extern bool Option_SetValue__SWIG_13(HandleRef jarg1, double jarg2);

	// Token: 0x06001011 RID: 4113
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_14")]
	public static extern bool Option_SetValue__SWIG_14(HandleRef jarg1, IntPtr jarg2, uint jarg3, bool jarg4);

	// Token: 0x06001012 RID: 4114
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_15")]
	public static extern bool Option_SetValue__SWIG_15(HandleRef jarg1, IntPtr jarg2, uint jarg3);

	// Token: 0x06001013 RID: 4115
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_16")]
	public static extern bool Option_SetValue__SWIG_16(HandleRef jarg1, HandleRef jarg2, bool jarg3);

	// Token: 0x06001014 RID: 4116
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_17")]
	public static extern bool Option_SetValue__SWIG_17(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06001015 RID: 4117
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_18")]
	public static extern bool Option_SetValue__SWIG_18(HandleRef jarg1, float jarg2, float jarg3, float jarg4, float jarg5, bool jarg6);

	// Token: 0x06001016 RID: 4118
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_19")]
	public static extern bool Option_SetValue__SWIG_19(HandleRef jarg1, float jarg2, float jarg3, float jarg4, float jarg5);

	// Token: 0x06001017 RID: 4119
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_20")]
	public static extern bool Option_SetValue__SWIG_20(HandleRef jarg1, HandleRef jarg2, bool jarg3);

	// Token: 0x06001018 RID: 4120
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_21")]
	public static extern bool Option_SetValue__SWIG_21(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06001019 RID: 4121
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_22")]
	public static extern bool Option_SetValue__SWIG_22(HandleRef jarg1, HandleRef jarg2, bool jarg3);

	// Token: 0x0600101A RID: 4122
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_23")]
	public static extern bool Option_SetValue__SWIG_23(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x0600101B RID: 4123
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_24")]
	public static extern bool Option_SetValue__SWIG_24(HandleRef jarg1, string jarg2, bool jarg3);

	// Token: 0x0600101C RID: 4124
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_SetValue__SWIG_25")]
	public static extern bool Option_SetValue__SWIG_25(HandleRef jarg1, string jarg2);

	// Token: 0x0600101D RID: 4125
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetValueStr")]
	public static extern string Option_GetValueStr(HandleRef jarg1);

	// Token: 0x0600101E RID: 4126
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetRawValueStr")]
	public static extern string Option_GetRawValueStr(HandleRef jarg1);

	// Token: 0x0600101F RID: 4127
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetValue__SWIG_0")]
	public static extern bool Option_GetValue__SWIG_0(HandleRef jarg1, out bool jarg2);

	// Token: 0x06001020 RID: 4128
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetValue__SWIG_1")]
	public static extern bool Option_GetValue__SWIG_1(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06001021 RID: 4129
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetValue__SWIG_2")]
	public static extern bool Option_GetValue__SWIG_2(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06001022 RID: 4130
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetValue__SWIG_3")]
	public static extern bool Option_GetValue__SWIG_3(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06001023 RID: 4131
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetValue__SWIG_4")]
	public static extern bool Option_GetValue__SWIG_4(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06001024 RID: 4132
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetValue__SWIG_5")]
	public static extern bool Option_GetValue__SWIG_5(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06001025 RID: 4133
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetValue__SWIG_6")]
	public static extern bool Option_GetValue__SWIG_6(HandleRef jarg1, out float jarg2);

	// Token: 0x06001026 RID: 4134
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetValue__SWIG_7")]
	public static extern bool Option_GetValue__SWIG_7(HandleRef jarg1, out double jarg2);

	// Token: 0x06001027 RID: 4135
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetValue__SWIG_8")]
	public static extern bool Option_GetValue__SWIG_8(HandleRef jarg1, IntPtr jarg2, uint jarg3);

	// Token: 0x06001028 RID: 4136
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetValue__SWIG_9")]
	public static extern bool Option_GetValue__SWIG_9(HandleRef jarg1, out float jarg2, out float jarg3, out float jarg4, out float jarg5);

	// Token: 0x06001029 RID: 4137
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetValue__SWIG_10")]
	public static extern bool Option_GetValue__SWIG_10(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x0600102A RID: 4138
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetValueSize")]
	public static extern uint Option_GetValueSize(HandleRef jarg1);

	// Token: 0x0600102B RID: 4139
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_Reset")]
	public static extern void Option_Reset(HandleRef jarg1);

	// Token: 0x0600102C RID: 4140
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_GetOptionStructData")]
	public static extern IntPtr Option_GetOptionStructData(HandleRef jarg1);

	// Token: 0x0600102D RID: 4141
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_RegisterOptionChangeHandler__SWIG_0")]
	public static extern void Option_RegisterOptionChangeHandler__SWIG_0(HandleRef jarg1, Void_UInt_UInt_UInt_Fn jarg2);

	// Token: 0x0600102E RID: 4142
	[DllImport("SDPCore", EntryPoint = "CSharp_Option_RegisterOptionChangeHandler__SWIG_1")]
	public static extern void Option_RegisterOptionChangeHandler__SWIG_1(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x0600102F RID: 4143
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionBool__SWIG_0")]
	public static extern IntPtr new_OptionBool__SWIG_0(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8, bool jarg9);

	// Token: 0x06001030 RID: 4144
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionBool__SWIG_1")]
	public static extern IntPtr new_OptionBool__SWIG_1(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8);

	// Token: 0x06001031 RID: 4145
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionBool__SWIG_2")]
	public static extern IntPtr new_OptionBool__SWIG_2(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7);

	// Token: 0x06001032 RID: 4146
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_OptionBool")]
	public static extern void delete_OptionBool(HandleRef jarg1);

	// Token: 0x06001033 RID: 4147
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionBool_SetValue__SWIG_0")]
	public static extern bool OptionBool_SetValue__SWIG_0(HandleRef jarg1, bool jarg2, bool jarg3);

	// Token: 0x06001034 RID: 4148
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionBool_SetValue__SWIG_1")]
	public static extern bool OptionBool_SetValue__SWIG_1(HandleRef jarg1, bool jarg2);

	// Token: 0x06001035 RID: 4149
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionBool_SetValue__SWIG_2")]
	public static extern bool OptionBool_SetValue__SWIG_2(HandleRef jarg1, string jarg2, bool jarg3);

	// Token: 0x06001036 RID: 4150
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionBool_SetValue__SWIG_3")]
	public static extern bool OptionBool_SetValue__SWIG_3(HandleRef jarg1, string jarg2);

	// Token: 0x06001037 RID: 4151
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionBool_GetValue")]
	public static extern bool OptionBool_GetValue(HandleRef jarg1, out bool jarg2);

	// Token: 0x06001038 RID: 4152
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionString__SWIG_0")]
	public static extern IntPtr new_OptionString__SWIG_0(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8, bool jarg9);

	// Token: 0x06001039 RID: 4153
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionString__SWIG_1")]
	public static extern IntPtr new_OptionString__SWIG_1(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8);

	// Token: 0x0600103A RID: 4154
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionString__SWIG_2")]
	public static extern IntPtr new_OptionString__SWIG_2(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7);

	// Token: 0x0600103B RID: 4155
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_OptionString")]
	public static extern void delete_OptionString(HandleRef jarg1);

	// Token: 0x0600103C RID: 4156
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionString_SetValue__SWIG_0")]
	public static extern bool OptionString_SetValue__SWIG_0(HandleRef jarg1, string jarg2, bool jarg3);

	// Token: 0x0600103D RID: 4157
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionString_SetValue__SWIG_1")]
	public static extern bool OptionString_SetValue__SWIG_1(HandleRef jarg1, string jarg2);

	// Token: 0x0600103E RID: 4158
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionString_GetValue")]
	public static extern bool OptionString_GetValue(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x0600103F RID: 4159
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionInt32__SWIG_0")]
	public static extern IntPtr new_OptionInt32__SWIG_0(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8, bool jarg9);

	// Token: 0x06001040 RID: 4160
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionInt32__SWIG_1")]
	public static extern IntPtr new_OptionInt32__SWIG_1(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8);

	// Token: 0x06001041 RID: 4161
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionInt32__SWIG_2")]
	public static extern IntPtr new_OptionInt32__SWIG_2(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7);

	// Token: 0x06001042 RID: 4162
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_OptionInt32")]
	public static extern void delete_OptionInt32(HandleRef jarg1);

	// Token: 0x06001043 RID: 4163
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionInt32_SetValue__SWIG_0")]
	public static extern bool OptionInt32_SetValue__SWIG_0(HandleRef jarg1, int jarg2, bool jarg3);

	// Token: 0x06001044 RID: 4164
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionInt32_SetValue__SWIG_1")]
	public static extern bool OptionInt32_SetValue__SWIG_1(HandleRef jarg1, int jarg2);

	// Token: 0x06001045 RID: 4165
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionInt32_SetValue__SWIG_2")]
	public static extern bool OptionInt32_SetValue__SWIG_2(HandleRef jarg1, string jarg2, bool jarg3);

	// Token: 0x06001046 RID: 4166
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionInt32_SetValue__SWIG_3")]
	public static extern bool OptionInt32_SetValue__SWIG_3(HandleRef jarg1, string jarg2);

	// Token: 0x06001047 RID: 4167
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionInt32_GetValue")]
	public static extern bool OptionInt32_GetValue(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06001048 RID: 4168
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionInt64__SWIG_0")]
	public static extern IntPtr new_OptionInt64__SWIG_0(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8, bool jarg9);

	// Token: 0x06001049 RID: 4169
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionInt64__SWIG_1")]
	public static extern IntPtr new_OptionInt64__SWIG_1(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8);

	// Token: 0x0600104A RID: 4170
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionInt64__SWIG_2")]
	public static extern IntPtr new_OptionInt64__SWIG_2(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7);

	// Token: 0x0600104B RID: 4171
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_OptionInt64")]
	public static extern void delete_OptionInt64(HandleRef jarg1);

	// Token: 0x0600104C RID: 4172
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionInt64_SetValue__SWIG_0")]
	public static extern bool OptionInt64_SetValue__SWIG_0(HandleRef jarg1, long jarg2, bool jarg3);

	// Token: 0x0600104D RID: 4173
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionInt64_SetValue__SWIG_1")]
	public static extern bool OptionInt64_SetValue__SWIG_1(HandleRef jarg1, long jarg2);

	// Token: 0x0600104E RID: 4174
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionInt64_SetValue__SWIG_2")]
	public static extern bool OptionInt64_SetValue__SWIG_2(HandleRef jarg1, string jarg2, bool jarg3);

	// Token: 0x0600104F RID: 4175
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionInt64_SetValue__SWIG_3")]
	public static extern bool OptionInt64_SetValue__SWIG_3(HandleRef jarg1, string jarg2);

	// Token: 0x06001050 RID: 4176
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionInt64_GetValue")]
	public static extern bool OptionInt64_GetValue(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06001051 RID: 4177
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionUInt32__SWIG_0")]
	public static extern IntPtr new_OptionUInt32__SWIG_0(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8, bool jarg9);

	// Token: 0x06001052 RID: 4178
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionUInt32__SWIG_1")]
	public static extern IntPtr new_OptionUInt32__SWIG_1(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8);

	// Token: 0x06001053 RID: 4179
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionUInt32__SWIG_2")]
	public static extern IntPtr new_OptionUInt32__SWIG_2(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7);

	// Token: 0x06001054 RID: 4180
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_OptionUInt32")]
	public static extern void delete_OptionUInt32(HandleRef jarg1);

	// Token: 0x06001055 RID: 4181
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionUInt32_SetValue__SWIG_0")]
	public static extern bool OptionUInt32_SetValue__SWIG_0(HandleRef jarg1, uint jarg2, bool jarg3);

	// Token: 0x06001056 RID: 4182
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionUInt32_SetValue__SWIG_1")]
	public static extern bool OptionUInt32_SetValue__SWIG_1(HandleRef jarg1, uint jarg2);

	// Token: 0x06001057 RID: 4183
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionUInt32_SetValue__SWIG_2")]
	public static extern bool OptionUInt32_SetValue__SWIG_2(HandleRef jarg1, string jarg2, bool jarg3);

	// Token: 0x06001058 RID: 4184
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionUInt32_SetValue__SWIG_3")]
	public static extern bool OptionUInt32_SetValue__SWIG_3(HandleRef jarg1, string jarg2);

	// Token: 0x06001059 RID: 4185
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionUInt32_GetValue")]
	public static extern bool OptionUInt32_GetValue(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x0600105A RID: 4186
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionUInt64__SWIG_0")]
	public static extern IntPtr new_OptionUInt64__SWIG_0(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8, bool jarg9);

	// Token: 0x0600105B RID: 4187
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionUInt64__SWIG_1")]
	public static extern IntPtr new_OptionUInt64__SWIG_1(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8);

	// Token: 0x0600105C RID: 4188
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionUInt64__SWIG_2")]
	public static extern IntPtr new_OptionUInt64__SWIG_2(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7);

	// Token: 0x0600105D RID: 4189
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_OptionUInt64")]
	public static extern void delete_OptionUInt64(HandleRef jarg1);

	// Token: 0x0600105E RID: 4190
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionUInt64_SetValue__SWIG_0")]
	public static extern bool OptionUInt64_SetValue__SWIG_0(HandleRef jarg1, ulong jarg2, bool jarg3);

	// Token: 0x0600105F RID: 4191
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionUInt64_SetValue__SWIG_1")]
	public static extern bool OptionUInt64_SetValue__SWIG_1(HandleRef jarg1, ulong jarg2);

	// Token: 0x06001060 RID: 4192
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionUInt64_SetValue__SWIG_2")]
	public static extern bool OptionUInt64_SetValue__SWIG_2(HandleRef jarg1, string jarg2, bool jarg3);

	// Token: 0x06001061 RID: 4193
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionUInt64_SetValue__SWIG_3")]
	public static extern bool OptionUInt64_SetValue__SWIG_3(HandleRef jarg1, string jarg2);

	// Token: 0x06001062 RID: 4194
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionUInt64_GetValue")]
	public static extern bool OptionUInt64_GetValue(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06001063 RID: 4195
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionFloat__SWIG_0")]
	public static extern IntPtr new_OptionFloat__SWIG_0(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8, bool jarg9);

	// Token: 0x06001064 RID: 4196
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionFloat__SWIG_1")]
	public static extern IntPtr new_OptionFloat__SWIG_1(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8);

	// Token: 0x06001065 RID: 4197
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionFloat__SWIG_2")]
	public static extern IntPtr new_OptionFloat__SWIG_2(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7);

	// Token: 0x06001066 RID: 4198
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_OptionFloat")]
	public static extern void delete_OptionFloat(HandleRef jarg1);

	// Token: 0x06001067 RID: 4199
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionFloat_SetValue__SWIG_0")]
	public static extern bool OptionFloat_SetValue__SWIG_0(HandleRef jarg1, float jarg2, bool jarg3);

	// Token: 0x06001068 RID: 4200
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionFloat_SetValue__SWIG_1")]
	public static extern bool OptionFloat_SetValue__SWIG_1(HandleRef jarg1, float jarg2);

	// Token: 0x06001069 RID: 4201
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionFloat_SetValue__SWIG_2")]
	public static extern bool OptionFloat_SetValue__SWIG_2(HandleRef jarg1, string jarg2, bool jarg3);

	// Token: 0x0600106A RID: 4202
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionFloat_SetValue__SWIG_3")]
	public static extern bool OptionFloat_SetValue__SWIG_3(HandleRef jarg1, string jarg2);

	// Token: 0x0600106B RID: 4203
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionFloat_GetValue")]
	public static extern bool OptionFloat_GetValue(HandleRef jarg1, out float jarg2);

	// Token: 0x0600106C RID: 4204
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionDouble__SWIG_0")]
	public static extern IntPtr new_OptionDouble__SWIG_0(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8, bool jarg9);

	// Token: 0x0600106D RID: 4205
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionDouble__SWIG_1")]
	public static extern IntPtr new_OptionDouble__SWIG_1(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8);

	// Token: 0x0600106E RID: 4206
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionDouble__SWIG_2")]
	public static extern IntPtr new_OptionDouble__SWIG_2(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7);

	// Token: 0x0600106F RID: 4207
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_OptionDouble")]
	public static extern void delete_OptionDouble(HandleRef jarg1);

	// Token: 0x06001070 RID: 4208
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionDouble_SetValue__SWIG_0")]
	public static extern bool OptionDouble_SetValue__SWIG_0(HandleRef jarg1, double jarg2, bool jarg3);

	// Token: 0x06001071 RID: 4209
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionDouble_SetValue__SWIG_1")]
	public static extern bool OptionDouble_SetValue__SWIG_1(HandleRef jarg1, double jarg2);

	// Token: 0x06001072 RID: 4210
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionDouble_SetValue__SWIG_2")]
	public static extern bool OptionDouble_SetValue__SWIG_2(HandleRef jarg1, string jarg2, bool jarg3);

	// Token: 0x06001073 RID: 4211
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionDouble_SetValue__SWIG_3")]
	public static extern bool OptionDouble_SetValue__SWIG_3(HandleRef jarg1, string jarg2);

	// Token: 0x06001074 RID: 4212
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionDouble_GetValue")]
	public static extern bool OptionDouble_GetValue(HandleRef jarg1, out double jarg2);

	// Token: 0x06001075 RID: 4213
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionBinary__SWIG_0")]
	public static extern IntPtr new_OptionBinary__SWIG_0(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, string jarg8, uint jarg9, HandleRef jarg10, bool jarg11);

	// Token: 0x06001076 RID: 4214
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionBinary__SWIG_1")]
	public static extern IntPtr new_OptionBinary__SWIG_1(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, string jarg8, uint jarg9, HandleRef jarg10);

	// Token: 0x06001077 RID: 4215
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionBinary__SWIG_2")]
	public static extern IntPtr new_OptionBinary__SWIG_2(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, string jarg8, uint jarg9);

	// Token: 0x06001078 RID: 4216
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_OptionBinary")]
	public static extern void delete_OptionBinary(HandleRef jarg1);

	// Token: 0x06001079 RID: 4217
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionBinary_SetValue__SWIG_0")]
	public static extern bool OptionBinary_SetValue__SWIG_0(HandleRef jarg1, string jarg2, bool jarg3);

	// Token: 0x0600107A RID: 4218
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionBinary_SetValue__SWIG_1")]
	public static extern bool OptionBinary_SetValue__SWIG_1(HandleRef jarg1, string jarg2);

	// Token: 0x0600107B RID: 4219
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionBinary_SetValue__SWIG_2")]
	public static extern bool OptionBinary_SetValue__SWIG_2(HandleRef jarg1, IntPtr jarg2, uint jarg3, bool jarg4);

	// Token: 0x0600107C RID: 4220
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionBinary_SetValue__SWIG_3")]
	public static extern bool OptionBinary_SetValue__SWIG_3(HandleRef jarg1, IntPtr jarg2, uint jarg3);

	// Token: 0x0600107D RID: 4221
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionBinary_GetValue")]
	public static extern bool OptionBinary_GetValue(HandleRef jarg1, IntPtr jarg2, uint jarg3);

	// Token: 0x0600107E RID: 4222
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionBinary_GetValueSize")]
	public static extern uint OptionBinary_GetValueSize(HandleRef jarg1);

	// Token: 0x0600107F RID: 4223
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionEnum__SWIG_0")]
	public static extern IntPtr new_OptionEnum__SWIG_0(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8, bool jarg9);

	// Token: 0x06001080 RID: 4224
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionEnum__SWIG_1")]
	public static extern IntPtr new_OptionEnum__SWIG_1(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8);

	// Token: 0x06001081 RID: 4225
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionEnum__SWIG_2")]
	public static extern IntPtr new_OptionEnum__SWIG_2(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7);

	// Token: 0x06001082 RID: 4226
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_OptionEnum")]
	public static extern void delete_OptionEnum(HandleRef jarg1);

	// Token: 0x06001083 RID: 4227
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionEnum_SetValue__SWIG_0")]
	public static extern bool OptionEnum_SetValue__SWIG_0(HandleRef jarg1, int jarg2, bool jarg3);

	// Token: 0x06001084 RID: 4228
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionEnum_SetValue__SWIG_1")]
	public static extern bool OptionEnum_SetValue__SWIG_1(HandleRef jarg1, int jarg2);

	// Token: 0x06001085 RID: 4229
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionEnum_SetValue__SWIG_2")]
	public static extern bool OptionEnum_SetValue__SWIG_2(HandleRef jarg1, string jarg2, bool jarg3);

	// Token: 0x06001086 RID: 4230
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionEnum_SetValue__SWIG_3")]
	public static extern bool OptionEnum_SetValue__SWIG_3(HandleRef jarg1, string jarg2);

	// Token: 0x06001087 RID: 4231
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionEnum_GetValue__SWIG_0")]
	public static extern bool OptionEnum_GetValue__SWIG_0(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06001088 RID: 4232
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionEnum_GetValue__SWIG_1")]
	public static extern bool OptionEnum_GetValue__SWIG_1(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06001089 RID: 4233
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionEnum_GetValueStr")]
	public static extern string OptionEnum_GetValueStr(HandleRef jarg1);

	// Token: 0x0600108A RID: 4234
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionColor__SWIG_0")]
	public static extern IntPtr new_OptionColor__SWIG_0(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8, bool jarg9);

	// Token: 0x0600108B RID: 4235
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionColor__SWIG_1")]
	public static extern IntPtr new_OptionColor__SWIG_1(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7, HandleRef jarg8);

	// Token: 0x0600108C RID: 4236
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionColor__SWIG_2")]
	public static extern IntPtr new_OptionColor__SWIG_2(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4, string jarg5, uint jarg6, uint jarg7);

	// Token: 0x0600108D RID: 4237
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_OptionColor")]
	public static extern void delete_OptionColor(HandleRef jarg1);

	// Token: 0x0600108E RID: 4238
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionColor_SetValue__SWIG_0")]
	public static extern bool OptionColor_SetValue__SWIG_0(HandleRef jarg1, HandleRef jarg2, bool jarg3);

	// Token: 0x0600108F RID: 4239
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionColor_SetValue__SWIG_1")]
	public static extern bool OptionColor_SetValue__SWIG_1(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x06001090 RID: 4240
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionColor_SetValue__SWIG_2")]
	public static extern bool OptionColor_SetValue__SWIG_2(HandleRef jarg1, string jarg2, bool jarg3);

	// Token: 0x06001091 RID: 4241
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionColor_SetValue__SWIG_3")]
	public static extern bool OptionColor_SetValue__SWIG_3(HandleRef jarg1, string jarg2);

	// Token: 0x06001092 RID: 4242
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionColor_SetValue__SWIG_4")]
	public static extern bool OptionColor_SetValue__SWIG_4(HandleRef jarg1, float jarg2, float jarg3, float jarg4, float jarg5, bool jarg6);

	// Token: 0x06001093 RID: 4243
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionColor_SetValue__SWIG_5")]
	public static extern bool OptionColor_SetValue__SWIG_5(HandleRef jarg1, float jarg2, float jarg3, float jarg4, float jarg5);

	// Token: 0x06001094 RID: 4244
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionColor_GetValue__SWIG_0")]
	public static extern bool OptionColor_GetValue__SWIG_0(HandleRef jarg1, out float jarg2);

	// Token: 0x06001095 RID: 4245
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionColor_GetValue__SWIG_1")]
	public static extern bool OptionColor_GetValue__SWIG_1(HandleRef jarg1, out float jarg2, out float jarg3, out float jarg4, out float jarg5);

	// Token: 0x06001096 RID: 4246
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionColor_GetValueStr")]
	public static extern string OptionColor_GetValueStr(HandleRef jarg1);

	// Token: 0x06001097 RID: 4247
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionStructDef")]
	public static extern IntPtr new_OptionStructDef(string jarg1);

	// Token: 0x06001098 RID: 4248
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructDef_AddAttribute__SWIG_0")]
	public static extern uint OptionStructDef_AddAttribute__SWIG_0(HandleRef jarg1, string jarg2, int jarg3);

	// Token: 0x06001099 RID: 4249
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructDef_AddAttribute__SWIG_1")]
	public static extern uint OptionStructDef_AddAttribute__SWIG_1(HandleRef jarg1, string jarg2, int jarg3, uint jarg4);

	// Token: 0x0600109A RID: 4250
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructDef_GetNumberAttributes")]
	public static extern uint OptionStructDef_GetNumberAttributes(HandleRef jarg1);

	// Token: 0x0600109B RID: 4251
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructDef_GetAttributeOffset")]
	public static extern uint OptionStructDef_GetAttributeOffset(HandleRef jarg1, uint jarg2);

	// Token: 0x0600109C RID: 4252
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructDef_GetAttributeType")]
	public static extern int OptionStructDef_GetAttributeType(HandleRef jarg1, uint jarg2);

	// Token: 0x0600109D RID: 4253
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructDef_GetAttributeName")]
	public static extern string OptionStructDef_GetAttributeName(HandleRef jarg1, uint jarg2);

	// Token: 0x0600109E RID: 4254
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructDef_NewData")]
	public static extern IntPtr OptionStructDef_NewData(HandleRef jarg1);

	// Token: 0x0600109F RID: 4255
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructDef_DeleteData")]
	public static extern void OptionStructDef_DeleteData(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060010A0 RID: 4256
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_OptionStructDef")]
	public static extern void delete_OptionStructDef(HandleRef jarg1);

	// Token: 0x060010A1 RID: 4257
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionStructData__SWIG_0")]
	public static extern IntPtr new_OptionStructData__SWIG_0(HandleRef jarg1);

	// Token: 0x060010A2 RID: 4258
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionStructData__SWIG_1")]
	public static extern IntPtr new_OptionStructData__SWIG_1(HandleRef jarg1);

	// Token: 0x060010A3 RID: 4259
	[DllImport("SDPCore", EntryPoint = "CSharp_new_OptionStructData__SWIG_2")]
	public static extern IntPtr new_OptionStructData__SWIG_2();

	// Token: 0x060010A4 RID: 4260
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_OptionStructData")]
	public static extern void delete_OptionStructData(HandleRef jarg1);

	// Token: 0x060010A5 RID: 4261
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructData_SetValue__SWIG_0")]
	public static extern bool OptionStructData_SetValue__SWIG_0(HandleRef jarg1, string jarg2, IntPtr jarg3);

	// Token: 0x060010A6 RID: 4262
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructData_SetValue__SWIG_1")]
	public static extern bool OptionStructData_SetValue__SWIG_1(HandleRef jarg1, uint jarg2, IntPtr jarg3);

	// Token: 0x060010A7 RID: 4263
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructData_SetValue__SWIG_2")]
	public static extern bool OptionStructData_SetValue__SWIG_2(HandleRef jarg1, string jarg2, string jarg3);

	// Token: 0x060010A8 RID: 4264
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructData_GetValue__SWIG_0")]
	public static extern string OptionStructData_GetValue__SWIG_0(HandleRef jarg1, string jarg2);

	// Token: 0x060010A9 RID: 4265
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructData_GetValue__SWIG_1")]
	public static extern bool OptionStructData_GetValue__SWIG_1(HandleRef jarg1, string jarg2, IntPtr jarg3);

	// Token: 0x060010AA RID: 4266
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructData_GetValue__SWIG_2")]
	public static extern bool OptionStructData_GetValue__SWIG_2(HandleRef jarg1, uint jarg2, IntPtr jarg3);

	// Token: 0x060010AB RID: 4267
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructData_GetSize")]
	public static extern uint OptionStructData_GetSize(HandleRef jarg1);

	// Token: 0x060010AC RID: 4268
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructData_GetDefinition")]
	public static extern IntPtr OptionStructData_GetDefinition(HandleRef jarg1);

	// Token: 0x060010AD RID: 4269
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStructData_Clone")]
	public static extern bool OptionStructData_Clone(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060010AE RID: 4270
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_OptionStruct")]
	public static extern void delete_OptionStruct(HandleRef jarg1);

	// Token: 0x060010AF RID: 4271
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStruct_SetValue__SWIG_0")]
	public static extern bool OptionStruct_SetValue__SWIG_0(HandleRef jarg1, HandleRef jarg2, bool jarg3);

	// Token: 0x060010B0 RID: 4272
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStruct_SetValue__SWIG_1")]
	public static extern bool OptionStruct_SetValue__SWIG_1(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060010B1 RID: 4273
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStruct_GetValue")]
	public static extern bool OptionStruct_GetValue(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060010B2 RID: 4274
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStruct_GetOptionStructData")]
	public static extern IntPtr OptionStruct_GetOptionStructData(HandleRef jarg1);

	// Token: 0x060010B3 RID: 4275
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStruct_GetOptionStructDef")]
	public static extern IntPtr OptionStruct_GetOptionStructDef(HandleRef jarg1);

	// Token: 0x060010B4 RID: 4276
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStruct_SetValue__SWIG_2")]
	public static extern bool OptionStruct_SetValue__SWIG_2(HandleRef jarg1, HandleRef jarg2, bool jarg3);

	// Token: 0x060010B5 RID: 4277
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStruct_SetValue__SWIG_3")]
	public static extern bool OptionStruct_SetValue__SWIG_3(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060010B6 RID: 4278
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestOptionCategories_providerID_set")]
	public static extern void RequestOptionCategories_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x060010B7 RID: 4279
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestOptionCategories_providerID_get")]
	public static extern uint RequestOptionCategories_providerID_get(HandleRef jarg1);

	// Token: 0x060010B8 RID: 4280
	[DllImport("SDPCore", EntryPoint = "CSharp_new_RequestOptionCategories")]
	public static extern IntPtr new_RequestOptionCategories(uint jarg1);

	// Token: 0x060010B9 RID: 4281
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_RequestOptionCategories")]
	public static extern void delete_RequestOptionCategories(HandleRef jarg1);

	// Token: 0x060010BA RID: 4282
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionCategory_providerID_set")]
	public static extern void ReplyOptionCategory_providerID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x060010BB RID: 4283
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionCategory_providerID_get")]
	public static extern uint ReplyOptionCategory_providerID_get(HandleRef jarg1);

	// Token: 0x060010BC RID: 4284
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionCategory_categoryID_set")]
	public static extern void ReplyOptionCategory_categoryID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x060010BD RID: 4285
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionCategory_categoryID_get")]
	public static extern uint ReplyOptionCategory_categoryID_get(HandleRef jarg1);

	// Token: 0x060010BE RID: 4286
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionCategory_parentCategoryID_set")]
	public static extern void ReplyOptionCategory_parentCategoryID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x060010BF RID: 4287
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionCategory_parentCategoryID_get")]
	public static extern uint ReplyOptionCategory_parentCategoryID_get(HandleRef jarg1);

	// Token: 0x060010C0 RID: 4288
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionCategory_name_set")]
	public static extern void ReplyOptionCategory_name_set(HandleRef jarg1, string jarg2);

	// Token: 0x060010C1 RID: 4289
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionCategory_name_get")]
	public static extern string ReplyOptionCategory_name_get(HandleRef jarg1);

	// Token: 0x060010C2 RID: 4290
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionCategory_description_set")]
	public static extern void ReplyOptionCategory_description_set(HandleRef jarg1, string jarg2);

	// Token: 0x060010C3 RID: 4291
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionCategory_description_get")]
	public static extern string ReplyOptionCategory_description_get(HandleRef jarg1);

	// Token: 0x060010C4 RID: 4292
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyOptionCategory__SWIG_0")]
	public static extern IntPtr new_ReplyOptionCategory__SWIG_0(uint jarg1, uint jarg2, string jarg3, string jarg4, uint jarg5);

	// Token: 0x060010C5 RID: 4293
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ReplyOptionCategory__SWIG_1")]
	public static extern IntPtr new_ReplyOptionCategory__SWIG_1(uint jarg1, uint jarg2, string jarg3, string jarg4);

	// Token: 0x060010C6 RID: 4294
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ReplyOptionCategory")]
	public static extern void delete_ReplyOptionCategory(HandleRef jarg1);

	// Token: 0x060010C7 RID: 4295
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_OptionCategory")]
	public static extern void delete_OptionCategory(HandleRef jarg1);

	// Token: 0x060010C8 RID: 4296
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionCategory_GetOptions")]
	public static extern IntPtr OptionCategory_GetOptions(HandleRef jarg1);

	// Token: 0x060010C9 RID: 4297
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionCategory_GetParentCategory")]
	public static extern IntPtr OptionCategory_GetParentCategory(HandleRef jarg1);

	// Token: 0x060010CA RID: 4298
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionCategory_GetSubCategories")]
	public static extern IntPtr OptionCategory_GetSubCategories(HandleRef jarg1);

	// Token: 0x060010CB RID: 4299
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionCategory_Publish")]
	public static extern bool OptionCategory_Publish(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060010CC RID: 4300
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionCategory_PublishStatus")]
	public static extern bool OptionCategory_PublishStatus(HandleRef jarg1);

	// Token: 0x060010CD RID: 4301
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionCategory_Update__SWIG_0")]
	public static extern void OptionCategory_Update__SWIG_0(HandleRef jarg1, string jarg2, string jarg3, HandleRef jarg4);

	// Token: 0x060010CE RID: 4302
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionCategory_Update__SWIG_1")]
	public static extern void OptionCategory_Update__SWIG_1(HandleRef jarg1, string jarg2, string jarg3);

	// Token: 0x060010CF RID: 4303
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionCategory_Clear")]
	public static extern void OptionCategory_Clear(HandleRef jarg1);

	// Token: 0x060010D0 RID: 4304
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPCoreInterop_GetMetricFromPointer")]
	public static extern IntPtr SDPCoreInterop_GetMetricFromPointer(IntPtr jarg1);

	// Token: 0x060010D1 RID: 4305
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPCoreInterop_GetFromPointer")]
	public static extern IntPtr SDPCoreInterop_GetFromPointer(IntPtr jarg1);

	// Token: 0x060010D2 RID: 4306
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPCoreInterop_GetStringFromCustomData__SWIG_0")]
	public static extern string SDPCoreInterop_GetStringFromCustomData__SWIG_0(IntPtr jarg1, uint jarg2);

	// Token: 0x060010D3 RID: 4307
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPCoreInterop_GetUInt32FromCustomData")]
	public static extern uint SDPCoreInterop_GetUInt32FromCustomData(IntPtr jarg1, uint jarg2);

	// Token: 0x060010D4 RID: 4308
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPCoreInterop_GetInt32FromCustomData")]
	public static extern int SDPCoreInterop_GetInt32FromCustomData(IntPtr jarg1, uint jarg2);

	// Token: 0x060010D5 RID: 4309
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPCoreInterop_GetStringFromCustomData__SWIG_1")]
	public static extern string SDPCoreInterop_GetStringFromCustomData__SWIG_1(IntPtr jarg1, string jarg2);

	// Token: 0x060010D6 RID: 4310
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPCoreInterop_ConvertUInt8PtrToPtr")]
	public static extern IntPtr SDPCoreInterop_ConvertUInt8PtrToPtr(HandleRef jarg1);

	// Token: 0x060010D7 RID: 4311
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPCoreInterop_ConvertPtrToDoubleValue")]
	public static extern double SDPCoreInterop_ConvertPtrToDoubleValue(IntPtr jarg1);

	// Token: 0x060010D8 RID: 4312
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPCoreInterop_GetIconSize")]
	public static extern uint SDPCoreInterop_GetIconSize();

	// Token: 0x060010D9 RID: 4313
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPCoreInterop_GetIconWidth")]
	public static extern uint SDPCoreInterop_GetIconWidth();

	// Token: 0x060010DA RID: 4314
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPCoreInterop_GetIconHeight")]
	public static extern uint SDPCoreInterop_GetIconHeight();

	// Token: 0x060010DB RID: 4315
	[DllImport("SDPCore", EntryPoint = "CSharp_new_SDPCoreInterop")]
	public static extern IntPtr new_SDPCoreInterop();

	// Token: 0x060010DC RID: 4316
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_SDPCoreInterop")]
	public static extern void delete_SDPCoreInterop(HandleRef jarg1);

	// Token: 0x060010DD RID: 4317
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_Error_NoDatabase")]
	public static extern long ModelObject_Error_NoDatabase();

	// Token: 0x060010DE RID: 4318
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ModelObject")]
	public static extern void delete_ModelObject(HandleRef jarg1);

	// Token: 0x060010DF RID: 4319
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_NewData")]
	public static extern IntPtr ModelObject_NewData(HandleRef jarg1);

	// Token: 0x060010E0 RID: 4320
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_Save")]
	public static extern long ModelObject_Save(HandleRef jarg1);

	// Token: 0x060010E1 RID: 4321
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_AddAttribute")]
	public static extern bool ModelObject_AddAttribute(HandleRef jarg1, string jarg2, int jarg3, uint jarg4, uint jarg5);

	// Token: 0x060010E2 RID: 4322
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_LinkAttribute")]
	public static extern bool ModelObject_LinkAttribute(HandleRef jarg1, string jarg2, HandleRef jarg3);

	// Token: 0x060010E3 RID: 4323
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_Insert")]
	public static extern long ModelObject_Insert(HandleRef jarg1, IntPtr jarg2);

	// Token: 0x060010E4 RID: 4324
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_GetData__SWIG_0")]
	public static extern IntPtr ModelObject_GetData__SWIG_0(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060010E5 RID: 4325
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_GetData__SWIG_1")]
	public static extern IntPtr ModelObject_GetData__SWIG_1(HandleRef jarg1);

	// Token: 0x060010E6 RID: 4326
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_GetDataByID")]
	public static extern IntPtr ModelObject_GetDataByID(HandleRef jarg1, long jarg2);

	// Token: 0x060010E7 RID: 4327
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_GetData__SWIG_2")]
	public static extern IntPtr ModelObject_GetData__SWIG_2(HandleRef jarg1, string jarg2, string jarg3);

	// Token: 0x060010E8 RID: 4328
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_GetData__SWIG_3")]
	public static extern IntPtr ModelObject_GetData__SWIG_3(HandleRef jarg1, string jarg2);

	// Token: 0x060010E9 RID: 4329
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_GetData__SWIG_4")]
	public static extern IntPtr ModelObject_GetData__SWIG_4(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060010EA RID: 4330
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_GetDataByAttribute__SWIG_0")]
	public static extern IntPtr ModelObject_GetDataByAttribute__SWIG_0(HandleRef jarg1, string jarg2, string jarg3);

	// Token: 0x060010EB RID: 4331
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_GetDataByAttribute__SWIG_1")]
	public static extern IntPtr ModelObject_GetDataByAttribute__SWIG_1(HandleRef jarg1, string jarg2);

	// Token: 0x060010EC RID: 4332
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_GetAll")]
	public static extern IntPtr ModelObject_GetAll(HandleRef jarg1);

	// Token: 0x060010ED RID: 4333
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_BeginBatch")]
	public static extern void ModelObject_BeginBatch();

	// Token: 0x060010EE RID: 4334
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObject_CommitBatch")]
	public static extern void ModelObject_CommitBatch();

	// Token: 0x060010EF RID: 4335
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ModelObjectData__SWIG_0")]
	public static extern IntPtr new_ModelObjectData__SWIG_0();

	// Token: 0x060010F0 RID: 4336
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ModelObjectData__SWIG_1")]
	public static extern IntPtr new_ModelObjectData__SWIG_1(HandleRef jarg1);

	// Token: 0x060010F1 RID: 4337
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ModelObjectData")]
	public static extern void delete_ModelObjectData(HandleRef jarg1);

	// Token: 0x060010F2 RID: 4338
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectData_Equal")]
	public static extern IntPtr ModelObjectData_Equal(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060010F3 RID: 4339
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectData_GetID")]
	public static extern long ModelObjectData_GetID(HandleRef jarg1);

	// Token: 0x060010F4 RID: 4340
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectData_IsValid")]
	public static extern bool ModelObjectData_IsValid(HandleRef jarg1);

	// Token: 0x060010F5 RID: 4341
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectData_Save")]
	public static extern long ModelObjectData_Save(HandleRef jarg1);

	// Token: 0x060010F6 RID: 4342
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectData_Update__SWIG_0")]
	public static extern bool ModelObjectData_Update__SWIG_0(HandleRef jarg1);

	// Token: 0x060010F7 RID: 4343
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectData_Update__SWIG_1")]
	public static extern bool ModelObjectData_Update__SWIG_1(HandleRef jarg1, IntPtr jarg2);

	// Token: 0x060010F8 RID: 4344
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectData_SetAttributeValue__SWIG_0")]
	public static extern bool ModelObjectData_SetAttributeValue__SWIG_0(HandleRef jarg1, string jarg2, string jarg3);

	// Token: 0x060010F9 RID: 4345
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectData_SetAttributeValue__SWIG_1")]
	public static extern bool ModelObjectData_SetAttributeValue__SWIG_1(HandleRef jarg1, string jarg2, IntPtr jarg3);

	// Token: 0x060010FA RID: 4346
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectData_GetValue")]
	public static extern string ModelObjectData_GetValue(HandleRef jarg1, string jarg2);

	// Token: 0x060010FB RID: 4347
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectData_GetData")]
	public static extern IntPtr ModelObjectData_GetData(HandleRef jarg1);

	// Token: 0x060010FC RID: 4348
	[DllImport("SDPCore", EntryPoint = "CSharp_ModelObjectData_GetValuePtrBinaryDataPair")]
	public static extern IntPtr ModelObjectData_GetValuePtrBinaryDataPair(HandleRef jarg1, string jarg2);

	// Token: 0x060010FD RID: 4349
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_Model")]
	public static extern void delete_Model(HandleRef jarg1);

	// Token: 0x060010FE RID: 4350
	[DllImport("SDPCore", EntryPoint = "CSharp_Model_GetName")]
	public static extern string Model_GetName(HandleRef jarg1);

	// Token: 0x060010FF RID: 4351
	[DllImport("SDPCore", EntryPoint = "CSharp_Model_GetMutex")]
	public static extern IntPtr Model_GetMutex(HandleRef jarg1);

	// Token: 0x06001100 RID: 4352
	[DllImport("SDPCore", EntryPoint = "CSharp_Model_AddObject")]
	public static extern IntPtr Model_AddObject(HandleRef jarg1, string jarg2);

	// Token: 0x06001101 RID: 4353
	[DllImport("SDPCore", EntryPoint = "CSharp_Model_GetModelObject")]
	public static extern IntPtr Model_GetModelObject(HandleRef jarg1, string jarg2);

	// Token: 0x06001102 RID: 4354
	[DllImport("SDPCore", EntryPoint = "CSharp_Model_RegisterOnModelChanged")]
	public static extern void Model_RegisterOnModelChanged(HandleRef jarg1);

	// Token: 0x06001103 RID: 4355
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_DataModel")]
	public static extern void delete_DataModel(HandleRef jarg1);

	// Token: 0x06001104 RID: 4356
	[DllImport("SDPCore", EntryPoint = "CSharp_DataModel_Get")]
	public static extern IntPtr DataModel_Get();

	// Token: 0x06001105 RID: 4357
	[DllImport("SDPCore", EntryPoint = "CSharp_DataModel_ShutDown")]
	public static extern void DataModel_ShutDown(HandleRef jarg1);

	// Token: 0x06001106 RID: 4358
	[DllImport("SDPCore", EntryPoint = "CSharp_DataModel_AddModel")]
	public static extern IntPtr DataModel_AddModel(HandleRef jarg1, string jarg2);

	// Token: 0x06001107 RID: 4359
	[DllImport("SDPCore", EntryPoint = "CSharp_DataModel_DeleteModel")]
	public static extern void DataModel_DeleteModel(HandleRef jarg1, string jarg2);

	// Token: 0x06001108 RID: 4360
	[DllImport("SDPCore", EntryPoint = "CSharp_DataModel_GetModel")]
	public static extern IntPtr DataModel_GetModel(HandleRef jarg1, string jarg2);

	// Token: 0x06001109 RID: 4361
	[DllImport("SDPCore", EntryPoint = "CSharp_DataModel_GetModelObject")]
	public static extern IntPtr DataModel_GetModelObject(HandleRef jarg1, HandleRef jarg2, string jarg3);

	// Token: 0x0600110A RID: 4362
	[DllImport("SDPCore", EntryPoint = "CSharp_DataModel_GetModelObjectData__SWIG_0")]
	public static extern IntPtr DataModel_GetModelObjectData__SWIG_0(HandleRef jarg1, HandleRef jarg2, long jarg3);

	// Token: 0x0600110B RID: 4363
	[DllImport("SDPCore", EntryPoint = "CSharp_DataModel_GetModelObjectData__SWIG_1")]
	public static extern IntPtr DataModel_GetModelObjectData__SWIG_1(HandleRef jarg1, HandleRef jarg2, string jarg3, string jarg4);

	// Token: 0x0600110C RID: 4364
	[DllImport("SDPCore", EntryPoint = "CSharp_BinaryData_capture_set")]
	public static extern void BinaryData_capture_set(HandleRef jarg1, uint jarg2);

	// Token: 0x0600110D RID: 4365
	[DllImport("SDPCore", EntryPoint = "CSharp_BinaryData_capture_get")]
	public static extern uint BinaryData_capture_get(HandleRef jarg1);

	// Token: 0x0600110E RID: 4366
	[DllImport("SDPCore", EntryPoint = "CSharp_BinaryData_bufferCategory_set")]
	public static extern void BinaryData_bufferCategory_set(HandleRef jarg1, uint jarg2);

	// Token: 0x0600110F RID: 4367
	[DllImport("SDPCore", EntryPoint = "CSharp_BinaryData_bufferCategory_get")]
	public static extern uint BinaryData_bufferCategory_get(HandleRef jarg1);

	// Token: 0x06001110 RID: 4368
	[DllImport("SDPCore", EntryPoint = "CSharp_BinaryData_bufferID_set")]
	public static extern void BinaryData_bufferID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06001111 RID: 4369
	[DllImport("SDPCore", EntryPoint = "CSharp_BinaryData_bufferID_get")]
	public static extern uint BinaryData_bufferID_get(HandleRef jarg1);

	// Token: 0x06001112 RID: 4370
	[DllImport("SDPCore", EntryPoint = "CSharp_new_BinaryData")]
	public static extern IntPtr new_BinaryData();

	// Token: 0x06001113 RID: 4371
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_BinaryData")]
	public static extern void delete_BinaryData(HandleRef jarg1);

	// Token: 0x06001114 RID: 4372
	[DllImport("SDPCore", EntryPoint = "CSharp_new_SessionModel")]
	public static extern IntPtr new_SessionModel();

	// Token: 0x06001115 RID: 4373
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_SessionModel")]
	public static extern void delete_SessionModel(HandleRef jarg1);

	// Token: 0x06001116 RID: 4374
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionModel_version_set")]
	public static extern void SessionModel_version_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06001117 RID: 4375
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionModel_version_get")]
	public static extern uint SessionModel_version_get(HandleRef jarg1);

	// Token: 0x06001118 RID: 4376
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionModel_versionString_set")]
	public static extern void SessionModel_versionString_set(HandleRef jarg1, string jarg2);

	// Token: 0x06001119 RID: 4377
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionModel_versionString_get")]
	public static extern string SessionModel_versionString_get(HandleRef jarg1);

	// Token: 0x0600111A RID: 4378
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionModel_buildDate_set")]
	public static extern void SessionModel_buildDate_set(HandleRef jarg1, string jarg2);

	// Token: 0x0600111B RID: 4379
	[DllImport("SDPCore", EntryPoint = "CSharp_SessionModel_buildDate_get")]
	public static extern string SessionModel_buildDate_get(HandleRef jarg1);

	// Token: 0x0600111C RID: 4380
	[DllImport("SDPCore", EntryPoint = "CSharp_new_CaptureModel")]
	public static extern IntPtr new_CaptureModel();

	// Token: 0x0600111D RID: 4381
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_CaptureModel")]
	public static extern void delete_CaptureModel(HandleRef jarg1);

	// Token: 0x0600111E RID: 4382
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_id_set")]
	public static extern void CaptureModel_id_set(HandleRef jarg1, uint jarg2);

	// Token: 0x0600111F RID: 4383
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_id_get")]
	public static extern uint CaptureModel_id_get(HandleRef jarg1);

	// Token: 0x06001120 RID: 4384
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_startTimeMono_set")]
	public static extern void CaptureModel_startTimeMono_set(HandleRef jarg1, long jarg2);

	// Token: 0x06001121 RID: 4385
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_startTimeMono_get")]
	public static extern long CaptureModel_startTimeMono_get(HandleRef jarg1);

	// Token: 0x06001122 RID: 4386
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_stopTimeMono_set")]
	public static extern void CaptureModel_stopTimeMono_set(HandleRef jarg1, long jarg2);

	// Token: 0x06001123 RID: 4387
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_stopTimeMono_get")]
	public static extern long CaptureModel_stopTimeMono_get(HandleRef jarg1);

	// Token: 0x06001124 RID: 4388
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_name_set")]
	public static extern void CaptureModel_name_set(HandleRef jarg1, string jarg2);

	// Token: 0x06001125 RID: 4389
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_name_get")]
	public static extern string CaptureModel_name_get(HandleRef jarg1);

	// Token: 0x06001126 RID: 4390
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_deviceID_set")]
	public static extern void CaptureModel_deviceID_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06001127 RID: 4391
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_deviceID_get")]
	public static extern uint CaptureModel_deviceID_get(HandleRef jarg1);

	// Token: 0x06001128 RID: 4392
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_device_set")]
	public static extern void CaptureModel_device_set(HandleRef jarg1, string jarg2);

	// Token: 0x06001129 RID: 4393
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_device_get")]
	public static extern string CaptureModel_device_get(HandleRef jarg1);

	// Token: 0x0600112A RID: 4394
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_captureType_set")]
	public static extern void CaptureModel_captureType_set(HandleRef jarg1, uint jarg2);

	// Token: 0x0600112B RID: 4395
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_captureType_get")]
	public static extern uint CaptureModel_captureType_get(HandleRef jarg1);

	// Token: 0x0600112C RID: 4396
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_startDelay_set")]
	public static extern void CaptureModel_startDelay_set(HandleRef jarg1, uint jarg2);

	// Token: 0x0600112D RID: 4397
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_startDelay_get")]
	public static extern uint CaptureModel_startDelay_get(HandleRef jarg1);

	// Token: 0x0600112E RID: 4398
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_duration_set")]
	public static extern void CaptureModel_duration_set(HandleRef jarg1, uint jarg2);

	// Token: 0x0600112F RID: 4399
	[DllImport("SDPCore", EntryPoint = "CSharp_CaptureModel_duration_get")]
	public static extern uint CaptureModel_duration_get(HandleRef jarg1);

	// Token: 0x06001130 RID: 4400
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ProcessModel")]
	public static extern IntPtr new_ProcessModel();

	// Token: 0x06001131 RID: 4401
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ProcessModel")]
	public static extern void delete_ProcessModel(HandleRef jarg1);

	// Token: 0x06001132 RID: 4402
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessModel_capture_set")]
	public static extern void ProcessModel_capture_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06001133 RID: 4403
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessModel_capture_get")]
	public static extern uint ProcessModel_capture_get(HandleRef jarg1);

	// Token: 0x06001134 RID: 4404
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessModel_pid_set")]
	public static extern void ProcessModel_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06001135 RID: 4405
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessModel_pid_get")]
	public static extern uint ProcessModel_pid_get(HandleRef jarg1);

	// Token: 0x06001136 RID: 4406
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessModel_name_set")]
	public static extern void ProcessModel_name_set(HandleRef jarg1, string jarg2);

	// Token: 0x06001137 RID: 4407
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessModel_name_get")]
	public static extern string ProcessModel_name_get(HandleRef jarg1);

	// Token: 0x06001138 RID: 4408
	[DllImport("SDPCore", EntryPoint = "CSharp_new_ThreadModel")]
	public static extern IntPtr new_ThreadModel();

	// Token: 0x06001139 RID: 4409
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ThreadModel")]
	public static extern void delete_ThreadModel(HandleRef jarg1);

	// Token: 0x0600113A RID: 4410
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadModel_pid_set")]
	public static extern void ThreadModel_pid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x0600113B RID: 4411
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadModel_pid_get")]
	public static extern uint ThreadModel_pid_get(HandleRef jarg1);

	// Token: 0x0600113C RID: 4412
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadModel_tid_set")]
	public static extern void ThreadModel_tid_set(HandleRef jarg1, uint jarg2);

	// Token: 0x0600113D RID: 4413
	[DllImport("SDPCore", EntryPoint = "CSharp_ThreadModel_tid_get")]
	public static extern uint ThreadModel_tid_get(HandleRef jarg1);

	// Token: 0x0600113E RID: 4414
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricModel")]
	public static extern IntPtr new_MetricModel();

	// Token: 0x0600113F RID: 4415
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_MetricModel")]
	public static extern void delete_MetricModel(HandleRef jarg1);

	// Token: 0x06001140 RID: 4416
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricModel_id_set")]
	public static extern void MetricModel_id_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06001141 RID: 4417
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricModel_id_get")]
	public static extern uint MetricModel_id_get(HandleRef jarg1);

	// Token: 0x06001142 RID: 4418
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricModel_name_set")]
	public static extern void MetricModel_name_set(HandleRef jarg1, string jarg2);

	// Token: 0x06001143 RID: 4419
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricModel_name_get")]
	public static extern string MetricModel_name_get(HandleRef jarg1);

	// Token: 0x06001144 RID: 4420
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricModel_category_set")]
	public static extern void MetricModel_category_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06001145 RID: 4421
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricModel_category_get")]
	public static extern uint MetricModel_category_get(HandleRef jarg1);

	// Token: 0x06001146 RID: 4422
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricModel_type_set")]
	public static extern void MetricModel_type_set(HandleRef jarg1, int jarg2);

	// Token: 0x06001147 RID: 4423
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricModel_type_get")]
	public static extern int MetricModel_type_get(HandleRef jarg1);

	// Token: 0x06001148 RID: 4424
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricModel_captureType_set")]
	public static extern void MetricModel_captureType_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06001149 RID: 4425
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricModel_captureType_get")]
	public static extern uint MetricModel_captureType_get(HandleRef jarg1);

	// Token: 0x0600114A RID: 4426
	[DllImport("SDPCore", EntryPoint = "CSharp_new_MetricCategoryModel")]
	public static extern IntPtr new_MetricCategoryModel();

	// Token: 0x0600114B RID: 4427
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_MetricCategoryModel")]
	public static extern void delete_MetricCategoryModel(HandleRef jarg1);

	// Token: 0x0600114C RID: 4428
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryModel_id_set")]
	public static extern void MetricCategoryModel_id_set(HandleRef jarg1, uint jarg2);

	// Token: 0x0600114D RID: 4429
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryModel_id_get")]
	public static extern uint MetricCategoryModel_id_get(HandleRef jarg1);

	// Token: 0x0600114E RID: 4430
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryModel_name_set")]
	public static extern void MetricCategoryModel_name_set(HandleRef jarg1, string jarg2);

	// Token: 0x0600114F RID: 4431
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryModel_name_get")]
	public static extern string MetricCategoryModel_name_get(HandleRef jarg1);

	// Token: 0x06001150 RID: 4432
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryModel_description_set")]
	public static extern void MetricCategoryModel_description_set(HandleRef jarg1, string jarg2);

	// Token: 0x06001151 RID: 4433
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricCategoryModel_description_get")]
	public static extern string MetricCategoryModel_description_get(HandleRef jarg1);

	// Token: 0x06001152 RID: 4434
	[DllImport("SDPCore", EntryPoint = "CSharp_new_DeviceModel")]
	public static extern IntPtr new_DeviceModel();

	// Token: 0x06001153 RID: 4435
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_DeviceModel")]
	public static extern void delete_DeviceModel(HandleRef jarg1);

	// Token: 0x06001154 RID: 4436
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_id_set")]
	public static extern void DeviceModel_id_set(HandleRef jarg1, uint jarg2);

	// Token: 0x06001155 RID: 4437
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_id_get")]
	public static extern uint DeviceModel_id_get(HandleRef jarg1);

	// Token: 0x06001156 RID: 4438
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_serialNumber_set")]
	public static extern void DeviceModel_serialNumber_set(HandleRef jarg1, string jarg2);

	// Token: 0x06001157 RID: 4439
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_serialNumber_get")]
	public static extern string DeviceModel_serialNumber_get(HandleRef jarg1);

	// Token: 0x06001158 RID: 4440
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_productName_set")]
	public static extern void DeviceModel_productName_set(HandleRef jarg1, string jarg2);

	// Token: 0x06001159 RID: 4441
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_productName_get")]
	public static extern string DeviceModel_productName_get(HandleRef jarg1);

	// Token: 0x0600115A RID: 4442
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_productModel_set")]
	public static extern void DeviceModel_productModel_set(HandleRef jarg1, string jarg2);

	// Token: 0x0600115B RID: 4443
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_productModel_get")]
	public static extern string DeviceModel_productModel_get(HandleRef jarg1);

	// Token: 0x0600115C RID: 4444
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_productManufacturer_set")]
	public static extern void DeviceModel_productManufacturer_set(HandleRef jarg1, string jarg2);

	// Token: 0x0600115D RID: 4445
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_productManufacturer_get")]
	public static extern string DeviceModel_productManufacturer_get(HandleRef jarg1);

	// Token: 0x0600115E RID: 4446
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_productBrand_set")]
	public static extern void DeviceModel_productBrand_set(HandleRef jarg1, string jarg2);

	// Token: 0x0600115F RID: 4447
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_productBrand_get")]
	public static extern string DeviceModel_productBrand_get(HandleRef jarg1);

	// Token: 0x06001160 RID: 4448
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_productLocaleRegion_set")]
	public static extern void DeviceModel_productLocaleRegion_set(HandleRef jarg1, string jarg2);

	// Token: 0x06001161 RID: 4449
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_productLocaleRegion_get")]
	public static extern string DeviceModel_productLocaleRegion_get(HandleRef jarg1);

	// Token: 0x06001162 RID: 4450
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_productLocaleLanguage_set")]
	public static extern void DeviceModel_productLocaleLanguage_set(HandleRef jarg1, string jarg2);

	// Token: 0x06001163 RID: 4451
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_productLocaleLanguage_get")]
	public static extern string DeviceModel_productLocaleLanguage_get(HandleRef jarg1);

	// Token: 0x06001164 RID: 4452
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_buildProduct_set")]
	public static extern void DeviceModel_buildProduct_set(HandleRef jarg1, string jarg2);

	// Token: 0x06001165 RID: 4453
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_buildProduct_get")]
	public static extern string DeviceModel_buildProduct_get(HandleRef jarg1);

	// Token: 0x06001166 RID: 4454
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_buildVersionRelease_set")]
	public static extern void DeviceModel_buildVersionRelease_set(HandleRef jarg1, string jarg2);

	// Token: 0x06001167 RID: 4455
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_buildVersionRelease_get")]
	public static extern string DeviceModel_buildVersionRelease_get(HandleRef jarg1);

	// Token: 0x06001168 RID: 4456
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_buildVersionSDK_set")]
	public static extern void DeviceModel_buildVersionSDK_set(HandleRef jarg1, string jarg2);

	// Token: 0x06001169 RID: 4457
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_buildVersionSDK_get")]
	public static extern string DeviceModel_buildVersionSDK_get(HandleRef jarg1);

	// Token: 0x0600116A RID: 4458
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_buildDate_set")]
	public static extern void DeviceModel_buildDate_set(HandleRef jarg1, string jarg2);

	// Token: 0x0600116B RID: 4459
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_buildDate_get")]
	public static extern string DeviceModel_buildDate_get(HandleRef jarg1);

	// Token: 0x0600116C RID: 4460
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_buildDescription_set")]
	public static extern void DeviceModel_buildDescription_set(HandleRef jarg1, string jarg2);

	// Token: 0x0600116D RID: 4461
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_buildDescription_get")]
	public static extern string DeviceModel_buildDescription_get(HandleRef jarg1);

	// Token: 0x0600116E RID: 4462
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_boardPlatform_set")]
	public static extern void DeviceModel_boardPlatform_set(HandleRef jarg1, string jarg2);

	// Token: 0x0600116F RID: 4463
	[DllImport("SDPCore", EntryPoint = "CSharp_DeviceModel_boardPlatform_get")]
	public static extern string DeviceModel_boardPlatform_get(HandleRef jarg1);

	// Token: 0x06001170 RID: 4464
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_REQUEST_get")]
	public static extern string OPT_REQUEST_get();

	// Token: 0x06001171 RID: 4465
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CAPTURE_ID_get")]
	public static extern string OPT_CAPTURE_ID_get();

	// Token: 0x06001172 RID: 4466
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DRAW_ID_get")]
	public static extern string OPT_DRAW_ID_get();

	// Token: 0x06001173 RID: 4467
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DRAW_MODE_get")]
	public static extern string OPT_DRAW_MODE_get();

	// Token: 0x06001174 RID: 4468
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SELECT_TEXTURE_get")]
	public static extern string OPT_SELECT_TEXTURE_get();

	// Token: 0x06001175 RID: 4469
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SELECT_PROGRAM_get")]
	public static extern string OPT_SELECT_PROGRAM_get();

	// Token: 0x06001176 RID: 4470
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SELECT_DRAW_ID_get")]
	public static extern string OPT_SELECT_DRAW_ID_get();

	// Token: 0x06001177 RID: 4471
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_ATTACHMENT_get")]
	public static extern string OPT_ATTACHMENT_get();

	// Token: 0x06001178 RID: 4472
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_TARGET_SELECTION_get")]
	public static extern string OPT_TARGET_SELECTION_get();

	// Token: 0x06001179 RID: 4473
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_THUMBNAIL_WIDTH_get")]
	public static extern string OPT_THUMBNAIL_WIDTH_get();

	// Token: 0x0600117A RID: 4474
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_THUMBNAIL_HEIGHT_get")]
	public static extern string OPT_THUMBNAIL_HEIGHT_get();

	// Token: 0x0600117B RID: 4475
	[DllImport("SDPCore", EntryPoint = "CSharp_DEFAULT_THUMBNAIL_WIDTH_get")]
	public static extern uint DEFAULT_THUMBNAIL_WIDTH_get();

	// Token: 0x0600117C RID: 4476
	[DllImport("SDPCore", EntryPoint = "CSharp_DEFAULT_THUMBNAIL_HEIGHT_get")]
	public static extern uint DEFAULT_THUMBNAIL_HEIGHT_get();

	// Token: 0x0600117D RID: 4477
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SELECT_LOCATION_X_get")]
	public static extern string OPT_SELECT_LOCATION_X_get();

	// Token: 0x0600117E RID: 4478
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SELECT_LOCATION_Y_get")]
	public static extern string OPT_SELECT_LOCATION_Y_get();

	// Token: 0x0600117F RID: 4479
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SELECT_LOCATION_VALID_get")]
	public static extern string OPT_SELECT_LOCATION_VALID_get();

	// Token: 0x06001180 RID: 4480
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_HISTORY_LOCATION_VALID_get")]
	public static extern string OPT_HISTORY_LOCATION_VALID_get();

	// Token: 0x06001181 RID: 4481
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_BUFFER_LIST_get")]
	public static extern string OPT_BUFFER_LIST_get();

	// Token: 0x06001182 RID: 4482
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SELECTED_CONTEXT_get")]
	public static extern string OPT_SELECTED_CONTEXT_get();

	// Token: 0x06001183 RID: 4483
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_RESPONSE_get")]
	public static extern string OPT_RESPONSE_get();

	// Token: 0x06001184 RID: 4484
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_WORK_STARTED_get")]
	public static extern string OPT_WORK_STARTED_get();

	// Token: 0x06001185 RID: 4485
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FAILURE_get")]
	public static extern string OPT_FAILURE_get();

	// Token: 0x06001186 RID: 4486
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORMAT_get")]
	public static extern string OPT_FORMAT_get();

	// Token: 0x06001187 RID: 4487
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_WIDTH_get")]
	public static extern string OPT_WIDTH_get();

	// Token: 0x06001188 RID: 4488
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_HEIGHT_get")]
	public static extern string OPT_HEIGHT_get();

	// Token: 0x06001189 RID: 4489
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_TARGET_SELECTED_get")]
	public static extern string OPT_TARGET_SELECTED_get();

	// Token: 0x0600118A RID: 4490
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DRAW_ID_SELECTED_get")]
	public static extern string OPT_DRAW_ID_SELECTED_get();

	// Token: 0x0600118B RID: 4491
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_PIXEL_HISTORY_get")]
	public static extern string OPT_PIXEL_HISTORY_get();

	// Token: 0x0600118C RID: 4492
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_CONTEXT_STATES_get")]
	public static extern string OPT_GL_CONTEXT_STATES_get();

	// Token: 0x0600118D RID: 4493
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SELECTED_CONTEXTS_get")]
	public static extern string OPT_SELECTED_CONTEXTS_get();

	// Token: 0x0600118E RID: 4494
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CONTEXT_OVERRIDE_MAKE_GLOBAL_get")]
	public static extern string OPT_CONTEXT_OVERRIDE_MAKE_GLOBAL_get();

	// Token: 0x0600118F RID: 4495
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CONTEXT_OVERRIDE_REMOVE_get")]
	public static extern string OPT_CONTEXT_OVERRIDE_REMOVE_get();

	// Token: 0x06001190 RID: 4496
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CONTEXT_OVERRIDE_REMOVE_ALL_get")]
	public static extern string OPT_CONTEXT_OVERRIDE_REMOVE_ALL_get();

	// Token: 0x06001191 RID: 4497
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_UPDATING_OPTIONS_get")]
	public static extern string OPT_UPDATING_OPTIONS_get();

	// Token: 0x06001192 RID: 4498
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GENERAL_STATES_get")]
	public static extern string OPT_GENERAL_STATES_get();

	// Token: 0x06001193 RID: 4499
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DITHER_ENABLE_get")]
	public static extern string OPT_DITHER_ENABLE_get();

	// Token: 0x06001194 RID: 4500
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_BLEND_STATES_get")]
	public static extern string OPT_BLEND_STATES_get();

	// Token: 0x06001195 RID: 4501
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_BLEND_ENABLE_get")]
	public static extern string OPT_BLEND_ENABLE_get();

	// Token: 0x06001196 RID: 4502
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_BLEND_EQUATION_RGB_get")]
	public static extern string OPT_BLEND_EQUATION_RGB_get();

	// Token: 0x06001197 RID: 4503
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_BLEND_EQUATION_ALPHA_get")]
	public static extern string OPT_BLEND_EQUATION_ALPHA_get();

	// Token: 0x06001198 RID: 4504
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_BLEND_FUNC_SRC_RGB_get")]
	public static extern string OPT_BLEND_FUNC_SRC_RGB_get();

	// Token: 0x06001199 RID: 4505
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_BLEND_FUNC_SRC_ALPHA_get")]
	public static extern string OPT_BLEND_FUNC_SRC_ALPHA_get();

	// Token: 0x0600119A RID: 4506
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_BLEND_FUNC_DEST_RGB_get")]
	public static extern string OPT_BLEND_FUNC_DEST_RGB_get();

	// Token: 0x0600119B RID: 4507
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_BLEND_FUNC_DEST_ALPHA_get")]
	public static extern string OPT_BLEND_FUNC_DEST_ALPHA_get();

	// Token: 0x0600119C RID: 4508
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_BLEND_COLOR_get")]
	public static extern string OPT_BLEND_COLOR_get();

	// Token: 0x0600119D RID: 4509
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CULL_STATES_get")]
	public static extern string OPT_CULL_STATES_get();

	// Token: 0x0600119E RID: 4510
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CULL_FACE_MODE_get")]
	public static extern string OPT_CULL_FACE_MODE_get();

	// Token: 0x0600119F RID: 4511
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CULL_FACE_ENABLE_get")]
	public static extern string OPT_CULL_FACE_ENABLE_get();

	// Token: 0x060011A0 RID: 4512
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEPTH_STATES_get")]
	public static extern string OPT_DEPTH_STATES_get();

	// Token: 0x060011A1 RID: 4513
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEPTH_FUNC_get")]
	public static extern string OPT_DEPTH_FUNC_get();

	// Token: 0x060011A2 RID: 4514
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEPTH_MASK_get")]
	public static extern string OPT_DEPTH_MASK_get();

	// Token: 0x060011A3 RID: 4515
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEPTH_TEST_ENABLE_get")]
	public static extern string OPT_DEPTH_TEST_ENABLE_get();

	// Token: 0x060011A4 RID: 4516
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CLEAR_DEPTH_get")]
	public static extern string OPT_CLEAR_DEPTH_get();

	// Token: 0x060011A5 RID: 4517
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEPTH_RANGE_NEAR_get")]
	public static extern string OPT_DEPTH_RANGE_NEAR_get();

	// Token: 0x060011A6 RID: 4518
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEPTH_RANGE_FAR_get")]
	public static extern string OPT_DEPTH_RANGE_FAR_get();

	// Token: 0x060011A7 RID: 4519
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_COLOR_STATES_get")]
	public static extern string OPT_COLOR_STATES_get();

	// Token: 0x060011A8 RID: 4520
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CLEAR_COLOR_get")]
	public static extern string OPT_CLEAR_COLOR_get();

	// Token: 0x060011A9 RID: 4521
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_COLOR_MASK_RED_get")]
	public static extern string OPT_COLOR_MASK_RED_get();

	// Token: 0x060011AA RID: 4522
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_COLOR_MASK_GREEN_get")]
	public static extern string OPT_COLOR_MASK_GREEN_get();

	// Token: 0x060011AB RID: 4523
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_COLOR_MASK_BLUE_get")]
	public static extern string OPT_COLOR_MASK_BLUE_get();

	// Token: 0x060011AC RID: 4524
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_COLOR_MASK_ALPHA_get")]
	public static extern string OPT_COLOR_MASK_ALPHA_get();

	// Token: 0x060011AD RID: 4525
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_STENCIL_STATES_get")]
	public static extern string OPT_STENCIL_STATES_get();

	// Token: 0x060011AE RID: 4526
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CLEAR_STENCIL_get")]
	public static extern string OPT_CLEAR_STENCIL_get();

	// Token: 0x060011AF RID: 4527
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_STENCIL_FUNC_FRONT_get")]
	public static extern string OPT_STENCIL_FUNC_FRONT_get();

	// Token: 0x060011B0 RID: 4528
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_STENCIL_FAIL_FRONT_get")]
	public static extern string OPT_STENCIL_FAIL_FRONT_get();

	// Token: 0x060011B1 RID: 4529
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_STENCIL_DEPTH_FAIL_FRONT_get")]
	public static extern string OPT_STENCIL_DEPTH_FAIL_FRONT_get();

	// Token: 0x060011B2 RID: 4530
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_STENCIL_DEPTH_PASS_FRONT_get")]
	public static extern string OPT_STENCIL_DEPTH_PASS_FRONT_get();

	// Token: 0x060011B3 RID: 4531
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_STENCIL_FUNC_BACK_get")]
	public static extern string OPT_STENCIL_FUNC_BACK_get();

	// Token: 0x060011B4 RID: 4532
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_STENCIL_FAIL_BACK_get")]
	public static extern string OPT_STENCIL_FAIL_BACK_get();

	// Token: 0x060011B5 RID: 4533
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_STENCIL_DEPTH_FAIL_BACK_get")]
	public static extern string OPT_STENCIL_DEPTH_FAIL_BACK_get();

	// Token: 0x060011B6 RID: 4534
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_STENCIL_DEPTH_PASS_BACK_get")]
	public static extern string OPT_STENCIL_DEPTH_PASS_BACK_get();

	// Token: 0x060011B7 RID: 4535
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_STENCIL_TEST_ENABLE_get")]
	public static extern string OPT_STENCIL_TEST_ENABLE_get();

	// Token: 0x060011B8 RID: 4536
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_STENCIL_REFERENCE_VALUE_FRONT_get")]
	public static extern string OPT_STENCIL_REFERENCE_VALUE_FRONT_get();

	// Token: 0x060011B9 RID: 4537
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_STENCIL_VALUE_MASK_FRONT_get")]
	public static extern string OPT_STENCIL_VALUE_MASK_FRONT_get();

	// Token: 0x060011BA RID: 4538
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_STENCIL_WRITE_MASK_FRONT_get")]
	public static extern string OPT_STENCIL_WRITE_MASK_FRONT_get();

	// Token: 0x060011BB RID: 4539
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_STENCIL_REFERENCE_VALUE_BACK_get")]
	public static extern string OPT_STENCIL_REFERENCE_VALUE_BACK_get();

	// Token: 0x060011BC RID: 4540
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_STENCIL_VALUE_MASK_BACK_get")]
	public static extern string OPT_STENCIL_VALUE_MASK_BACK_get();

	// Token: 0x060011BD RID: 4541
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_STENCIL_WRITE_MASK_BACK_get")]
	public static extern string OPT_STENCIL_WRITE_MASK_BACK_get();

	// Token: 0x060011BE RID: 4542
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_LINE_STATES_get")]
	public static extern string OPT_LINE_STATES_get();

	// Token: 0x060011BF RID: 4543
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_LINE_WIDTH_get")]
	public static extern string OPT_LINE_WIDTH_get();

	// Token: 0x060011C0 RID: 4544
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_POLYGON_OFFSET_STATES_get")]
	public static extern string OPT_POLYGON_OFFSET_STATES_get();

	// Token: 0x060011C1 RID: 4545
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_POLYGON_OFFSET_FILL_ENABLE_get")]
	public static extern string OPT_POLYGON_OFFSET_FILL_ENABLE_get();

	// Token: 0x060011C2 RID: 4546
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_POLYGONOFFSET_FACTOR_get")]
	public static extern string OPT_POLYGONOFFSET_FACTOR_get();

	// Token: 0x060011C3 RID: 4547
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_POLYGONOFFSET_UNITS_get")]
	public static extern string OPT_POLYGONOFFSET_UNITS_get();

	// Token: 0x060011C4 RID: 4548
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SAMPLE_COVERAGE_STATES_get")]
	public static extern string OPT_SAMPLE_COVERAGE_STATES_get();

	// Token: 0x060011C5 RID: 4549
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SAMPLECOVERAGE_VALUE_get")]
	public static extern string OPT_SAMPLECOVERAGE_VALUE_get();

	// Token: 0x060011C6 RID: 4550
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SAMPLECOVERAGE_INVERT_get")]
	public static extern string OPT_SAMPLECOVERAGE_INVERT_get();

	// Token: 0x060011C7 RID: 4551
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SAMPLE_ALPHA_TO_COVERAGE_ENABLE_get")]
	public static extern string OPT_SAMPLE_ALPHA_TO_COVERAGE_ENABLE_get();

	// Token: 0x060011C8 RID: 4552
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SAMPLE_COVERAGE_ENABLE_get")]
	public static extern string OPT_SAMPLE_COVERAGE_ENABLE_get();

	// Token: 0x060011C9 RID: 4553
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SAMPLE_MASK_ENABLE_get")]
	public static extern string OPT_SAMPLE_MASK_ENABLE_get();

	// Token: 0x060011CA RID: 4554
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SCISSOR_STATES_get")]
	public static extern string OPT_SCISSOR_STATES_get();

	// Token: 0x060011CB RID: 4555
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SCISSOR_TEST_ENABLE_get")]
	public static extern string OPT_SCISSOR_TEST_ENABLE_get();

	// Token: 0x060011CC RID: 4556
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SCISSOR_X_get")]
	public static extern string OPT_SCISSOR_X_get();

	// Token: 0x060011CD RID: 4557
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SCISSOR_Y_get")]
	public static extern string OPT_SCISSOR_Y_get();

	// Token: 0x060011CE RID: 4558
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SCISSOR_WIDTH_get")]
	public static extern string OPT_SCISSOR_WIDTH_get();

	// Token: 0x060011CF RID: 4559
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SCISSOR_HEIGHT_get")]
	public static extern string OPT_SCISSOR_HEIGHT_get();

	// Token: 0x060011D0 RID: 4560
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_PIXEL_PACKING_get")]
	public static extern string OPT_PIXEL_PACKING_get();

	// Token: 0x060011D1 RID: 4561
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_PIXEL_STORE_PACKALIGNMENT_get")]
	public static extern string OPT_PIXEL_STORE_PACKALIGNMENT_get();

	// Token: 0x060011D2 RID: 4562
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_PIXEL_STORE_PACKUNALIGNMENT_get")]
	public static extern string OPT_PIXEL_STORE_PACKUNALIGNMENT_get();

	// Token: 0x060011D3 RID: 4563
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_ACTIVE_STATES_get")]
	public static extern string OPT_ACTIVE_STATES_get();

	// Token: 0x060011D4 RID: 4564
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_ACTIVE_PROGRAM_get")]
	public static extern string OPT_ACTIVE_PROGRAM_get();

	// Token: 0x060011D5 RID: 4565
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_ACTIVE_PIPELINE_get")]
	public static extern string OPT_ACTIVE_PIPELINE_get();

	// Token: 0x060011D6 RID: 4566
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_ACTIVE_TEXTURE_UNIT_get")]
	public static extern string OPT_ACTIVE_TEXTURE_UNIT_get();

	// Token: 0x060011D7 RID: 4567
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_VIEWPORT_STATES_get")]
	public static extern string OPT_VIEWPORT_STATES_get();

	// Token: 0x060011D8 RID: 4568
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_VIEWPORT_X_get")]
	public static extern string OPT_VIEWPORT_X_get();

	// Token: 0x060011D9 RID: 4569
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_VIEWPORT_Y_get")]
	public static extern string OPT_VIEWPORT_Y_get();

	// Token: 0x060011DA RID: 4570
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_VIEWPORT_WIDTH_get")]
	public static extern string OPT_VIEWPORT_WIDTH_get();

	// Token: 0x060011DB RID: 4571
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_VIEWPORT_HEIGHT_get")]
	public static extern string OPT_VIEWPORT_HEIGHT_get();

	// Token: 0x060011DC RID: 4572
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_PROGRAM_OVERRIDE_get")]
	public static extern string OPT_PROGRAM_OVERRIDE_get();

	// Token: 0x060011DD RID: 4573
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_OPTION_STRUCT_DEF_get")]
	public static extern string OPT_OPTION_STRUCT_DEF_get();

	// Token: 0x060011DE RID: 4574
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CONTEXT_ID_get")]
	public static extern string OPT_CONTEXT_ID_get();

	// Token: 0x060011DF RID: 4575
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_PROGRAM_ID_get")]
	public static extern string OPT_PROGRAM_ID_get();

	// Token: 0x060011E0 RID: 4576
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_ENABLE_get")]
	public static extern string OPT_ENABLE_get();

	// Token: 0x060011E1 RID: 4577
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_PROGRAM_SEPARABLE_get")]
	public static extern string OPT_PROGRAM_SEPARABLE_get();

	// Token: 0x060011E2 RID: 4578
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_VERTEX_SHADER_get")]
	public static extern string OPT_VERTEX_SHADER_get();

	// Token: 0x060011E3 RID: 4579
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FRAGMENT_SHADER_get")]
	public static extern string OPT_FRAGMENT_SHADER_get();

	// Token: 0x060011E4 RID: 4580
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_COMPUTE_SHADER_get")]
	public static extern string OPT_COMPUTE_SHADER_get();

	// Token: 0x060011E5 RID: 4581
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GEOMETRY_SHADER_get")]
	public static extern string OPT_GEOMETRY_SHADER_get();

	// Token: 0x060011E6 RID: 4582
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_TESSCTRL_SHADER_get")]
	public static extern string OPT_TESSCTRL_SHADER_get();

	// Token: 0x060011E7 RID: 4583
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_TESSEVAL_SHADER_get")]
	public static extern string OPT_TESSEVAL_SHADER_get();

	// Token: 0x060011E8 RID: 4584
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DRAWCALL_SKIP_LIST_get")]
	public static extern string OPT_DRAWCALL_SKIP_LIST_get();

	// Token: 0x060011E9 RID: 4585
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_DEBUG_OPTIONS_get")]
	public static extern string OPT_GL_DEBUG_OPTIONS_get();

	// Token: 0x060011EA RID: 4586
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_CATEGORY_get")]
	public static extern string OPT_DEBUG_CATEGORY_get();

	// Token: 0x060011EB RID: 4587
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ENABLE_CONTEXT_SELECTION_OUTPUT_get")]
	public static extern string OPT_DEBUG_ENABLE_CONTEXT_SELECTION_OUTPUT_get();

	// Token: 0x060011EC RID: 4588
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ENABLE_SHADER_CUSTOMIZATION_get")]
	public static extern string OPT_DEBUG_ENABLE_SHADER_CUSTOMIZATION_get();

	// Token: 0x060011ED RID: 4589
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ENABLE_SHADER_CUSTOMIZATION_DEBUG_OUTPUT_get")]
	public static extern string OPT_DEBUG_ENABLE_SHADER_CUSTOMIZATION_DEBUG_OUTPUT_get();

	// Token: 0x060011EE RID: 4590
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ENABLE_OVERRIDE_TOKENS_get")]
	public static extern string OPT_DEBUG_ENABLE_OVERRIDE_TOKENS_get();

	// Token: 0x060011EF RID: 4591
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ENABLE_REPORTING_TOKENS_get")]
	public static extern string OPT_DEBUG_ENABLE_REPORTING_TOKENS_get();

	// Token: 0x060011F0 RID: 4592
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ENABLE_REPORTING_TOKEN_DEBUG_OUTPUT_get")]
	public static extern string OPT_DEBUG_ENABLE_REPORTING_TOKEN_DEBUG_OUTPUT_get();

	// Token: 0x060011F1 RID: 4593
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ENABLE_SCREENSHOT_TOKEN_DEBUG_OUTPUT_get")]
	public static extern string OPT_DEBUG_ENABLE_SCREENSHOT_TOKEN_DEBUG_OUTPUT_get();

	// Token: 0x060011F2 RID: 4594
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ENABLE_TOKEN_DEBUG_OUTPUT_get")]
	public static extern string OPT_DEBUG_ENABLE_TOKEN_DEBUG_OUTPUT_get();

	// Token: 0x060011F3 RID: 4595
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ENABLE_THUMBNAIL_SCREENSHOT_get")]
	public static extern string OPT_DEBUG_ENABLE_THUMBNAIL_SCREENSHOT_get();

	// Token: 0x060011F4 RID: 4596
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_THUMBNAIL_SHRINK_FACTOR_get")]
	public static extern string OPT_DEBUG_THUMBNAIL_SHRINK_FACTOR_get();

	// Token: 0x060011F5 RID: 4597
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_PIXEL_HISTORY_RADIUS_get")]
	public static extern string OPT_DEBUG_PIXEL_HISTORY_RADIUS_get();

	// Token: 0x060011F6 RID: 4598
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ENABLE_HIGHLIGHT_SCREENSHOT_get")]
	public static extern string OPT_DEBUG_ENABLE_HIGHLIGHT_SCREENSHOT_get();

	// Token: 0x060011F7 RID: 4599
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ENABLE_DEPTHSTENCIL_SCREENSHOT_get")]
	public static extern string OPT_DEBUG_ENABLE_DEPTHSTENCIL_SCREENSHOT_get();

	// Token: 0x060011F8 RID: 4600
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ENABLE_FULL_SCREENSHOT_get")]
	public static extern string OPT_DEBUG_ENABLE_FULL_SCREENSHOT_get();

	// Token: 0x060011F9 RID: 4601
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ENABLE_FULL_DCAP_get")]
	public static extern string OPT_DEBUG_ENABLE_FULL_DCAP_get();

	// Token: 0x060011FA RID: 4602
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_TEST_PIXEL_HISTORY_get")]
	public static extern string OPT_DEBUG_TEST_PIXEL_HISTORY_get();

	// Token: 0x060011FB RID: 4603
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ENABLE_OPTION_CHANGE_DEBUG_OUTPUT_get")]
	public static extern string OPT_DEBUG_ENABLE_OPTION_CHANGE_DEBUG_OUTPUT_get();

	// Token: 0x060011FC RID: 4604
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ENABLE_BUFFER_ON_DEMAND_get")]
	public static extern string OPT_DEBUG_ENABLE_BUFFER_ON_DEMAND_get();

	// Token: 0x060011FD RID: 4605
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ENABLE_CLIENT_REPLAY_CACHING_get")]
	public static extern string OPT_DEBUG_ENABLE_CLIENT_REPLAY_CACHING_get();

	// Token: 0x060011FE RID: 4606
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_DISABLE_SNAPSHOT_get")]
	public static extern string OPT_DEBUG_DISABLE_SNAPSHOT_get();

	// Token: 0x060011FF RID: 4607
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_DISABLE_BINARY_SHADERS_get")]
	public static extern string OPT_DEBUG_DISABLE_BINARY_SHADERS_get();

	// Token: 0x06001200 RID: 4608
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ALLOW_PROGRAM_OVERRIDES_get")]
	public static extern string OPT_DEBUG_ALLOW_PROGRAM_OVERRIDES_get();

	// Token: 0x06001201 RID: 4609
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ALLOW_STD_DELIMITERS_get")]
	public static extern string OPT_DEBUG_ALLOW_STD_DELIMITERS_get();

	// Token: 0x06001202 RID: 4610
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_ALLOW_DRAWCALL_DELIMITERS_get")]
	public static extern string OPT_DEBUG_ALLOW_DRAWCALL_DELIMITERS_get();

	// Token: 0x06001203 RID: 4611
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DEBUG_USE_LOCAL_SNAPSHOT_FILE_get")]
	public static extern string OPT_DEBUG_USE_LOCAL_SNAPSHOT_FILE_get();

	// Token: 0x06001204 RID: 4612
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GLES_INFORMATION_get")]
	public static extern string OPT_GLES_INFORMATION_get();

	// Token: 0x06001205 RID: 4613
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_INFO_EGL_VENDOR_get")]
	public static extern string OPT_INFO_EGL_VENDOR_get();

	// Token: 0x06001206 RID: 4614
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_INFO_EGL_VERSION_get")]
	public static extern string OPT_INFO_EGL_VERSION_get();

	// Token: 0x06001207 RID: 4615
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_INFO_GL_VENDOR_get")]
	public static extern string OPT_INFO_GL_VENDOR_get();

	// Token: 0x06001208 RID: 4616
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_INFO_GL_RENDERER_get")]
	public static extern string OPT_INFO_GL_RENDERER_get();

	// Token: 0x06001209 RID: 4617
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_INFO_GL_VERSION_get")]
	public static extern string OPT_INFO_GL_VERSION_get();

	// Token: 0x0600120A RID: 4618
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_INFO_GLSL_VERSION_get")]
	public static extern string OPT_INFO_GLSL_VERSION_get();

	// Token: 0x0600120B RID: 4619
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_APPLICATION_INFORMATION_get")]
	public static extern string OPT_APPLICATION_INFORMATION_get();

	// Token: 0x0600120C RID: 4620
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GENERAL_INFORMATION_get")]
	public static extern string OPT_GENERAL_INFORMATION_get();

	// Token: 0x0600120D RID: 4621
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_APPLICATION_NAME_get")]
	public static extern string OPT_APPLICATION_NAME_get();

	// Token: 0x0600120E RID: 4622
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_APPLICATION_PROCESS_ID_get")]
	public static extern string OPT_APPLICATION_PROCESS_ID_get();

	// Token: 0x0600120F RID: 4623
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GPU_IDENTIFIER_get")]
	public static extern string OPT_GPU_IDENTIFIER_get();

	// Token: 0x06001210 RID: 4624
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_ADAPTER_DESCRIPTION_get")]
	public static extern string OPT_ADAPTER_DESCRIPTION_get();

	// Token: 0x06001211 RID: 4625
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_CAPABILITY_INFORMATION_get")]
	public static extern string OPT_GL_CAPABILITY_INFORMATION_get();

	// Token: 0x06001212 RID: 4626
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CAN_CANCEL_GL_SNAPSHOT_get")]
	public static extern string OPT_CAN_CANCEL_GL_SNAPSHOT_get();

	// Token: 0x06001213 RID: 4627
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CPU_METRICS_get")]
	public static extern string OPT_CPU_METRICS_get();

	// Token: 0x06001214 RID: 4628
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CPU_CATEGORY_SAMPLING_get")]
	public static extern string OPT_CPU_CATEGORY_SAMPLING_get();

	// Token: 0x06001215 RID: 4629
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CPU_REALTIME_UPDATE_PERIOD_get")]
	public static extern string OPT_CPU_REALTIME_UPDATE_PERIOD_get();

	// Token: 0x06001216 RID: 4630
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CPU_TRACE_UPDATE_PERIOD_get")]
	public static extern string OPT_CPU_TRACE_UPDATE_PERIOD_get();

	// Token: 0x06001217 RID: 4631
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GGPM_GPU_METRICS_get")]
	public static extern string OPT_GGPM_GPU_METRICS_get();

	// Token: 0x06001218 RID: 4632
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_QGL_DISABLE_PLAYBACK_OPT_get")]
	public static extern string OPT_QGL_DISABLE_PLAYBACK_OPT_get();

	// Token: 0x06001219 RID: 4633
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CATEGORY_SAMPLING_get")]
	public static extern string OPT_CATEGORY_SAMPLING_get();

	// Token: 0x0600121A RID: 4634
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_REALTIME_UPDATE_PERIOD_get")]
	public static extern string OPT_REALTIME_UPDATE_PERIOD_get();

	// Token: 0x0600121B RID: 4635
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_TRACE_UPDATE_PERIOD_get")]
	public static extern string OPT_TRACE_UPDATE_PERIOD_get();

	// Token: 0x0600121C RID: 4636
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_OPENCL_CAPTURE_OPTIONS_get")]
	public static extern string OPT_OPENCL_CAPTURE_OPTIONS_get();

	// Token: 0x0600121D RID: 4637
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_ENABLE_BUFFER_IMAGE_TRANSMISSION_get")]
	public static extern string OPT_ENABLE_BUFFER_IMAGE_TRANSMISSION_get();

	// Token: 0x0600121E RID: 4638
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_TERMINATE_ON_RELEASE_CONTEXT_get")]
	public static extern string OPT_TERMINATE_ON_RELEASE_CONTEXT_get();

	// Token: 0x0600121F RID: 4639
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_ENABLE_BLOCKING_get")]
	public static extern string OPT_ENABLE_BLOCKING_get();

	// Token: 0x06001220 RID: 4640
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_LAUNCH_SUSPENDED_OPENCL_get")]
	public static extern string OPT_LAUNCH_SUSPENDED_OPENCL_get();

	// Token: 0x06001221 RID: 4641
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_OPENCL_INFORMATION_get")]
	public static extern string OPT_OPENCL_INFORMATION_get();

	// Token: 0x06001222 RID: 4642
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_OPENCL_DRIVER_VERSION_get")]
	public static extern string OPT_OPENCL_DRIVER_VERSION_get();

	// Token: 0x06001223 RID: 4643
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CL_VENDOR_get")]
	public static extern string OPT_CL_VENDOR_get();

	// Token: 0x06001224 RID: 4644
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CL_VERSION_get")]
	public static extern string OPT_CL_VERSION_get();

	// Token: 0x06001225 RID: 4645
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_REQUEST_KERNEL_STATS_get")]
	public static extern string OPT_REQUEST_KERNEL_STATS_get();

	// Token: 0x06001226 RID: 4646
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_KERNEL_ANALYZER_SEND_DISASM_get")]
	public static extern string OPT_KERNEL_ANALYZER_SEND_DISASM_get();

	// Token: 0x06001227 RID: 4647
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_KERNEL_ANALYZER_SEND_DEBUG_INFO_get")]
	public static extern string OPT_KERNEL_ANALYZER_SEND_DEBUG_INFO_get();

	// Token: 0x06001228 RID: 4648
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_LAUNCH_KERNEL_ANALYZER_get")]
	public static extern string OPT_LAUNCH_KERNEL_ANALYZER_get();

	// Token: 0x06001229 RID: 4649
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SNAPSHOT_FROM_GFXR_FILE_get")]
	public static extern string OPT_SNAPSHOT_FROM_GFXR_FILE_get();

	// Token: 0x0600122A RID: 4650
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_VULKAN_SNAPSHOT_FROM_SESSION_get")]
	public static extern string OPT_VULKAN_SNAPSHOT_FROM_SESSION_get();

	// Token: 0x0600122B RID: 4651
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_VULKAN_CATEGORY_get")]
	public static extern string OPT_VULKAN_CATEGORY_get();

	// Token: 0x0600122C RID: 4652
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_VULKAN_REPLAY_REQUEST_get")]
	public static extern string OPT_VULKAN_REPLAY_REQUEST_get();

	// Token: 0x0600122D RID: 4653
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_VULKAN_REALTIME_OPTIONS_get")]
	public static extern string OPT_VULKAN_REALTIME_OPTIONS_get();

	// Token: 0x0600122E RID: 4654
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FRAME_TIME_PERIOD_get")]
	public static extern string OPT_FRAME_TIME_PERIOD_get();

	// Token: 0x0600122F RID: 4655
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_VULKAN_SHADER_PROFILING_SUPPORT_get")]
	public static extern string OPT_VULKAN_SHADER_PROFILING_SUPPORT_get();

	// Token: 0x06001230 RID: 4656
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_VULKAN_SNAPSHOT_get")]
	public static extern string OPT_VULKAN_SNAPSHOT_get();

	// Token: 0x06001231 RID: 4657
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_VULKAN_LAUNCH_SUSPENDED_get")]
	public static extern string OPT_VULKAN_LAUNCH_SUSPENDED_get();

	// Token: 0x06001232 RID: 4658
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DX11_REALTIME_OPTIONS_get")]
	public static extern string OPT_DX11_REALTIME_OPTIONS_get();

	// Token: 0x06001233 RID: 4659
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DX12_SNAPSHOT_get")]
	public static extern string OPT_DX12_SNAPSHOT_get();

	// Token: 0x06001234 RID: 4660
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_ENABLE_OPTIMIZATION_OPT_get")]
	public static extern string OPT_ENABLE_OPTIMIZATION_OPT_get();

	// Token: 0x06001235 RID: 4661
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DX12_SNAPSHOT_FROM_SESSION_get")]
	public static extern string OPT_DX12_SNAPSHOT_FROM_SESSION_get();

	// Token: 0x06001236 RID: 4662
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_API_OPTION_CATEGORY_NAME_get")]
	public static extern string OPT_API_OPTION_CATEGORY_NAME_get();

	// Token: 0x06001237 RID: 4663
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DX12_REPLAY_REQUEST_get")]
	public static extern string OPT_DX12_REPLAY_REQUEST_get();

	// Token: 0x06001238 RID: 4664
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DX12_LAUNCH_SUSPENDED_get")]
	public static extern string OPT_DX12_LAUNCH_SUSPENDED_get();

	// Token: 0x06001239 RID: 4665
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_OPENGL_GENERAL_OPTIONS_get")]
	public static extern string OPT_OPENGL_GENERAL_OPTIONS_get();

	// Token: 0x0600123A RID: 4666
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_OPENGL_SNAPSHOT_FROM_SESSION_get")]
	public static extern string OPT_OPENGL_SNAPSHOT_FROM_SESSION_get();

	// Token: 0x0600123B RID: 4667
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_SHADER_STATS_REQUEST_get")]
	public static extern string OPT_SHADER_STATS_REQUEST_get();

	// Token: 0x0600123C RID: 4668
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_OPENGL_GENERAL_OVERRIDES_get")]
	public static extern string OPT_OPENGL_GENERAL_OVERRIDES_get();

	// Token: 0x0600123D RID: 4669
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_USE_AUTO_FRAME_DELIMITERS_get")]
	public static extern string OPT_USE_AUTO_FRAME_DELIMITERS_get();

	// Token: 0x0600123E RID: 4670
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FRAME_DELIMITER_PERIOD_get")]
	public static extern string OPT_FRAME_DELIMITER_PERIOD_get();

	// Token: 0x0600123F RID: 4671
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_NUMBER_OF_CAPTURE_FRAMES_get")]
	public static extern string OPT_NUMBER_OF_CAPTURE_FRAMES_get();

	// Token: 0x06001240 RID: 4672
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CAPTURE_TIME_get")]
	public static extern string OPT_CAPTURE_TIME_get();

	// Token: 0x06001241 RID: 4673
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_CAPTURE_TIMEOUT_get")]
	public static extern string OPT_CAPTURE_TIMEOUT_get();

	// Token: 0x06001242 RID: 4674
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_REALTIME_OVERRIDES_get")]
	public static extern string OPT_GL_REALTIME_OVERRIDES_get();

	// Token: 0x06001243 RID: 4675
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_EGL_OVERRIDE_CATEGORY_get")]
	public static extern string OPT_EGL_OVERRIDE_CATEGORY_get();

	// Token: 0x06001244 RID: 4676
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DISABLE_EGL_CALLS_get")]
	public static extern string OPT_DISABLE_EGL_CALLS_get();

	// Token: 0x06001245 RID: 4677
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DISABLE_EGLSWAPBUFFERS_get")]
	public static extern string OPT_DISABLE_EGLSWAPBUFFERS_get();

	// Token: 0x06001246 RID: 4678
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FINISH_ON_SWAP_get")]
	public static extern string OPT_FINISH_ON_SWAP_get();

	// Token: 0x06001247 RID: 4679
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_OVERRIDE_CATEGORY_get")]
	public static extern string OPT_GL_OVERRIDE_CATEGORY_get();

	// Token: 0x06001248 RID: 4680
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DISABLE_GL_get")]
	public static extern string OPT_DISABLE_GL_get();

	// Token: 0x06001249 RID: 4681
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DISABLE_DRAW_ARRAYS_get")]
	public static extern string OPT_DISABLE_DRAW_ARRAYS_get();

	// Token: 0x0600124A RID: 4682
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DISABLE_DRAW_ELEMENTS_get")]
	public static extern string OPT_DISABLE_DRAW_ELEMENTS_get();

	// Token: 0x0600124B RID: 4683
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DISABLE_READ_PIXELS_get")]
	public static extern string OPT_DISABLE_READ_PIXELS_get();

	// Token: 0x0600124C RID: 4684
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DISABLE_FINISH_get")]
	public static extern string OPT_DISABLE_FINISH_get();

	// Token: 0x0600124D RID: 4685
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DISABLE_FLUSH_get")]
	public static extern string OPT_DISABLE_FLUSH_get();

	// Token: 0x0600124E RID: 4686
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DISABLE_COLOR_CLEARS_get")]
	public static extern string OPT_DISABLE_COLOR_CLEARS_get();

	// Token: 0x0600124F RID: 4687
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DISABLE_DEPTH_CLEARS_get")]
	public static extern string OPT_DISABLE_DEPTH_CLEARS_get();

	// Token: 0x06001250 RID: 4688
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DISABLE_STENCIL_CLEARS_get")]
	public static extern string OPT_DISABLE_STENCIL_CLEARS_get();

	// Token: 0x06001251 RID: 4689
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_VERTEX_OVERRIDE_CATEGORY_get")]
	public static extern string OPT_GL_VERTEX_OVERRIDE_CATEGORY_get();

	// Token: 0x06001252 RID: 4690
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DISABLE_VBO_RENDERING_get")]
	public static extern string OPT_DISABLE_VBO_RENDERING_get();

	// Token: 0x06001253 RID: 4691
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_DISABLE_NON_VBO_RENDERING_get")]
	public static extern string OPT_DISABLE_NON_VBO_RENDERING_get();

	// Token: 0x06001254 RID: 4692
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_CULLING_OFF_get")]
	public static extern string OPT_FORCE_CULLING_OFF_get();

	// Token: 0x06001255 RID: 4693
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_CULLING_REJECT_get")]
	public static extern string OPT_FORCE_CULLING_REJECT_get();

	// Token: 0x06001256 RID: 4694
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_CULLING_BACK_get")]
	public static extern string OPT_FORCE_CULLING_BACK_get();

	// Token: 0x06001257 RID: 4695
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_CULLING_FRONT_get")]
	public static extern string OPT_FORCE_CULLING_FRONT_get();

	// Token: 0x06001258 RID: 4696
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_FRAGMENT_OVERRIDE_CATEGORY_get")]
	public static extern string OPT_GL_FRAGMENT_OVERRIDE_CATEGORY_get();

	// Token: 0x06001259 RID: 4697
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_SCISSOR_TEST_REJECT_get")]
	public static extern string OPT_FORCE_SCISSOR_TEST_REJECT_get();

	// Token: 0x0600125A RID: 4698
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_BLENDING_OFF_get")]
	public static extern string OPT_FORCE_BLENDING_OFF_get();

	// Token: 0x0600125B RID: 4699
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_STENCIL_OFF_get")]
	public static extern string OPT_FORCE_STENCIL_OFF_get();

	// Token: 0x0600125C RID: 4700
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_STENCIL_TESTING_OFF_get")]
	public static extern string OPT_FORCE_STENCIL_TESTING_OFF_get();

	// Token: 0x0600125D RID: 4701
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_STENCIL_TEST_REJECT_get")]
	public static extern string OPT_FORCE_STENCIL_TEST_REJECT_get();

	// Token: 0x0600125E RID: 4702
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_DEPTH_TEST_OFF_get")]
	public static extern string OPT_FORCE_DEPTH_TEST_OFF_get();

	// Token: 0x0600125F RID: 4703
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_DEPTH_TEST_REJECT_get")]
	public static extern string OPT_FORCE_DEPTH_TEST_REJECT_get();

	// Token: 0x06001260 RID: 4704
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_COLOR_MASK_OFF_get")]
	public static extern string OPT_FORCE_COLOR_MASK_OFF_get();

	// Token: 0x06001261 RID: 4705
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_SMALL_VIEWPORT_get")]
	public static extern string OPT_FORCE_SMALL_VIEWPORT_get();

	// Token: 0x06001262 RID: 4706
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_MULTI_SAMPLING_OFF_get")]
	public static extern string OPT_FORCE_MULTI_SAMPLING_OFF_get();

	// Token: 0x06001263 RID: 4707
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_TEXTURE_OVERRIDE_CATEGORY_get")]
	public static extern string OPT_GL_TEXTURE_OVERRIDE_CATEGORY_get();

	// Token: 0x06001264 RID: 4708
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_SMALL_TEXTURES_get")]
	public static extern string OPT_FORCE_SMALL_TEXTURES_get();

	// Token: 0x06001265 RID: 4709
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_DRIVER_OVERRIDE_CATEGORY_get")]
	public static extern string OPT_GL_DRIVER_OVERRIDE_CATEGORY_get();

	// Token: 0x06001266 RID: 4710
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_IFD_get")]
	public static extern string OPT_FORCE_IFD_get();

	// Token: 0x06001267 RID: 4711
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_FORCE_IFH_get")]
	public static extern string OPT_FORCE_IFH_get();

	// Token: 0x06001268 RID: 4712
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_POWER_OVERRIDE_CATEGORY_get")]
	public static extern string OPT_POWER_OVERRIDE_CATEGORY_get();

	// Token: 0x06001269 RID: 4713
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_ENABLE_GPU_DCVS_AND_NAP_get")]
	public static extern string OPT_ENABLE_GPU_DCVS_AND_NAP_get();

	// Token: 0x0600126A RID: 4714
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_OPENGL_FRAME_DELIMITERS_CATEGORY_get")]
	public static extern string OPT_OPENGL_FRAME_DELIMITERS_CATEGORY_get();

	// Token: 0x0600126B RID: 4715
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_EGL_FRAME_DELIMITER_CATEGORY_get")]
	public static extern string OPT_EGL_FRAME_DELIMITER_CATEGORY_get();

	// Token: 0x0600126C RID: 4716
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_EGL_SWAP_BUFFERS_get")]
	public static extern string OPT_EGL_SWAP_BUFFERS_get();

	// Token: 0x0600126D RID: 4717
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_EGL_MAKE_CURRENT_get")]
	public static extern string OPT_EGL_MAKE_CURRENT_get();

	// Token: 0x0600126E RID: 4718
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_EGL_COPY_BUFFERS_get")]
	public static extern string OPT_EGL_COPY_BUFFERS_get();

	// Token: 0x0600126F RID: 4719
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_EGL_WAIT_GL_get")]
	public static extern string OPT_EGL_WAIT_GL_get();

	// Token: 0x06001270 RID: 4720
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_EGL_WAIT_NATIVE_get")]
	public static extern string OPT_EGL_WAIT_NATIVE_get();

	// Token: 0x06001271 RID: 4721
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_EGL_WAIT_CLIENT_get")]
	public static extern string OPT_EGL_WAIT_CLIENT_get();

	// Token: 0x06001272 RID: 4722
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_EGL_BIND_API_get")]
	public static extern string OPT_EGL_BIND_API_get();

	// Token: 0x06001273 RID: 4723
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_EGL_LOCK_SURFACE_KHR_get")]
	public static extern string OPT_EGL_LOCK_SURFACE_KHR_get();

	// Token: 0x06001274 RID: 4724
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_EGL_UNLOCK_SURFACE_KHR_get")]
	public static extern string OPT_EGL_UNLOCK_SURFACE_KHR_get();

	// Token: 0x06001275 RID: 4725
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_FRAME_DELIMITER_CATEGORY_get")]
	public static extern string OPT_GL_FRAME_DELIMITER_CATEGORY_get();

	// Token: 0x06001276 RID: 4726
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_CLEAR_get")]
	public static extern string OPT_GL_CLEAR_get();

	// Token: 0x06001277 RID: 4727
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_FINISH_get")]
	public static extern string OPT_GL_FINISH_get();

	// Token: 0x06001278 RID: 4728
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_FLUSH_get")]
	public static extern string OPT_GL_FLUSH_get();

	// Token: 0x06001279 RID: 4729
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_INSERT_EVENT_MARKER_EXT_get")]
	public static extern string OPT_GL_INSERT_EVENT_MARKER_EXT_get();

	// Token: 0x0600127A RID: 4730
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_WAIT_SYNC_get")]
	public static extern string OPT_GL_WAIT_SYNC_get();

	// Token: 0x0600127B RID: 4731
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_CLIENT_WAIT_SYNC_get")]
	public static extern string OPT_GL_CLIENT_WAIT_SYNC_get();

	// Token: 0x0600127C RID: 4732
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_START_TILING_QCOM_get")]
	public static extern string OPT_GL_START_TILING_QCOM_get();

	// Token: 0x0600127D RID: 4733
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_END_TILING_QCOM_get")]
	public static extern string OPT_GL_END_TILING_QCOM_get();

	// Token: 0x0600127E RID: 4734
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_DRAW_get")]
	public static extern string OPT_GL_DRAW_get();

	// Token: 0x0600127F RID: 4735
	[DllImport("SDPCore", EntryPoint = "CSharp_OPT_GL_DEBUG_MESSAGE_INSERT_get")]
	public static extern string OPT_GL_DEBUG_MESSAGE_INSERT_get();

	// Token: 0x06001280 RID: 4736
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_DSP_MODE_ADSP_get")]
	public static extern int SDP_DSP_MODE_ADSP_get();

	// Token: 0x06001281 RID: 4737
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_DSP_MODE_MDSP_get")]
	public static extern int SDP_DSP_MODE_MDSP_get();

	// Token: 0x06001282 RID: 4738
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_DSP_MODE_SDSP_get")]
	public static extern int SDP_DSP_MODE_SDSP_get();

	// Token: 0x06001283 RID: 4739
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_DSP_MODE_CDSP_get")]
	public static extern int SDP_DSP_MODE_CDSP_get();

	// Token: 0x06001284 RID: 4740
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_NPU_MODE_get")]
	public static extern int SDP_NPU_MODE_get();

	// Token: 0x06001285 RID: 4741
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_DSP_MODE_CDSP1_get")]
	public static extern int SDP_DSP_MODE_CDSP1_get();

	// Token: 0x06001286 RID: 4742
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_DATA_PROVIDER_NAME_LIST_get")]
	public static extern IntPtr DSP_DATA_PROVIDER_NAME_LIST_get();

	// Token: 0x06001287 RID: 4743
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_DATA_PROVIDER_DESC_LIST_get")]
	public static extern IntPtr DSP_DATA_PROVIDER_DESC_LIST_get();

	// Token: 0x06001288 RID: 4744
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_DATA_PROVIDER_DEV_NODE_LIST_get")]
	public static extern IntPtr DSP_DATA_PROVIDER_DEV_NODE_LIST_get();

	// Token: 0x06001289 RID: 4745
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_METRIC_CATEGORY_NAME_LIST_get")]
	public static extern IntPtr DSP_METRIC_CATEGORY_NAME_LIST_get();

	// Token: 0x0600128A RID: 4746
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_METRIC_PREFIX_LIST_get")]
	public static extern IntPtr DSP_METRIC_PREFIX_LIST_get();

	// Token: 0x0600128B RID: 4747
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_NAME_LIST_get")]
	public static extern IntPtr DSP_NAME_LIST_get();

	// Token: 0x0600128C RID: 4748
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_METRIC_EFFQ6_get")]
	public static extern string DSP_METRIC_EFFQ6_get();

	// Token: 0x0600128D RID: 4749
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_METRIC_CORECLOCK_get")]
	public static extern string DSP_METRIC_CORECLOCK_get();

	// Token: 0x0600128E RID: 4750
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_METRIC_BUSCLOCKVOTE_get")]
	public static extern string DSP_METRIC_BUSCLOCKVOTE_get();

	// Token: 0x0600128F RID: 4751
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_METRIC_1TACTIVE_get")]
	public static extern string DSP_METRIC_1TACTIVE_get();

	// Token: 0x06001290 RID: 4752
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_METRIC_L2FETCH_DU_MISS_get")]
	public static extern string DSP_METRIC_L2FETCH_DU_MISS_get();

	// Token: 0x06001291 RID: 4753
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_METRIC_MPPS_get")]
	public static extern string DSP_METRIC_MPPS_get();

	// Token: 0x06001292 RID: 4754
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_METRIC_PCPP_get")]
	public static extern string DSP_METRIC_PCPP_get();

	// Token: 0x06001293 RID: 4755
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_METRIC_HVXTHREADMPPS_get")]
	public static extern string DSP_METRIC_HVXTHREADMPPS_get();

	// Token: 0x06001294 RID: 4756
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_METRIC_HVXMPPS_get")]
	public static extern string DSP_METRIC_HVXMPPS_get();

	// Token: 0x06001295 RID: 4757
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_METRIC_AXIRDBW_get")]
	public static extern string DSP_METRIC_AXIRDBW_get();

	// Token: 0x06001296 RID: 4758
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_METRIC_AXIWRBW_get")]
	public static extern string DSP_METRIC_AXIWRBW_get();

	// Token: 0x06001297 RID: 4759
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_METRIC_HMX_UTILZ_get")]
	public static extern string DSP_METRIC_HMX_UTILZ_get();

	// Token: 0x06001298 RID: 4760
	[DllImport("SDPCore", EntryPoint = "CSharp_NPU_METRIC_DDRRDBW_get")]
	public static extern string NPU_METRIC_DDRRDBW_get();

	// Token: 0x06001299 RID: 4761
	[DllImport("SDPCore", EntryPoint = "CSharp_NPU_METRIC_DDRWRBW_get")]
	public static extern string NPU_METRIC_DDRWRBW_get();

	// Token: 0x0600129A RID: 4762
	[DllImport("SDPCore", EntryPoint = "CSharp_NPU_METRIC_DDRRDBW1_get")]
	public static extern string NPU_METRIC_DDRRDBW1_get();

	// Token: 0x0600129B RID: 4763
	[DllImport("SDPCore", EntryPoint = "CSharp_NPU_METRIC_DDRWRBW1_get")]
	public static extern string NPU_METRIC_DDRWRBW1_get();

	// Token: 0x0600129C RID: 4764
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_COUNT_get")]
	public static extern int DSP_COUNT_get();

	// Token: 0x0600129D RID: 4765
	[DllImport("SDPCore", EntryPoint = "CSharp_ADSP_get")]
	public static extern int ADSP_get();

	// Token: 0x0600129E RID: 4766
	[DllImport("SDPCore", EntryPoint = "CSharp_SDSP_get")]
	public static extern int SDSP_get();

	// Token: 0x0600129F RID: 4767
	[DllImport("SDPCore", EntryPoint = "CSharp_CDSP_get")]
	public static extern int CDSP_get();

	// Token: 0x060012A0 RID: 4768
	[DllImport("SDPCore", EntryPoint = "CSharp_CDSP1_get")]
	public static extern int CDSP1_get();

	// Token: 0x060012A1 RID: 4769
	[DllImport("SDPCore", EntryPoint = "CSharp_NPU_get")]
	public static extern int NPU_get();

	// Token: 0x060012A2 RID: 4770
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_METRIC_CATEGORY_NAME_get")]
	public static extern string DSP_METRIC_CATEGORY_NAME_get();

	// Token: 0x060012A3 RID: 4771
	[DllImport("SDPCore", EntryPoint = "CSharp_SLPI_METRIC_CATEGORY_NAME_get")]
	public static extern string SLPI_METRIC_CATEGORY_NAME_get();

	// Token: 0x060012A4 RID: 4772
	[DllImport("SDPCore", EntryPoint = "CSharp_CDSP_METRIC_CATEGORY_NAME_get")]
	public static extern string CDSP_METRIC_CATEGORY_NAME_get();

	// Token: 0x060012A5 RID: 4773
	[DllImport("SDPCore", EntryPoint = "CSharp_CDSP1_METRIC_CATEGORY_NAME_get")]
	public static extern string CDSP1_METRIC_CATEGORY_NAME_get();

	// Token: 0x060012A6 RID: 4774
	[DllImport("SDPCore", EntryPoint = "CSharp_NPU_METRIC_CATEGORY_NAME_get")]
	public static extern string NPU_METRIC_CATEGORY_NAME_get();

	// Token: 0x060012A7 RID: 4775
	[DllImport("SDPCore", EntryPoint = "CSharp_ADSP_MODEL_NAME_get")]
	public static extern string ADSP_MODEL_NAME_get();

	// Token: 0x060012A8 RID: 4776
	[DllImport("SDPCore", EntryPoint = "CSharp_ADSP_MODEL_COUNTERS_NAME_get")]
	public static extern string ADSP_MODEL_COUNTERS_NAME_get();

	// Token: 0x060012A9 RID: 4777
	[DllImport("SDPCore", EntryPoint = "CSharp_SDSP_MODEL_NAME_get")]
	public static extern string SDSP_MODEL_NAME_get();

	// Token: 0x060012AA RID: 4778
	[DllImport("SDPCore", EntryPoint = "CSharp_SDSP_MODEL_COUNTERS_NAME_get")]
	public static extern string SDSP_MODEL_COUNTERS_NAME_get();

	// Token: 0x060012AB RID: 4779
	[DllImport("SDPCore", EntryPoint = "CSharp_CDSP_MODEL_NAME_get")]
	public static extern string CDSP_MODEL_NAME_get();

	// Token: 0x060012AC RID: 4780
	[DllImport("SDPCore", EntryPoint = "CSharp_CDSP_MODEL_TABLE_COUNTERS_NAME_get")]
	public static extern string CDSP_MODEL_TABLE_COUNTERS_NAME_get();

	// Token: 0x060012AD RID: 4781
	[DllImport("SDPCore", EntryPoint = "CSharp_NPU_MODEL_NAME_get")]
	public static extern string NPU_MODEL_NAME_get();

	// Token: 0x060012AE RID: 4782
	[DllImport("SDPCore", EntryPoint = "CSharp_NPU_MODEL_TABLE_COUNTERS_NAME_get")]
	public static extern string NPU_MODEL_TABLE_COUNTERS_NAME_get();

	// Token: 0x060012AF RID: 4783
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_MODEL_ATTRIB_TIMESTAMP_get")]
	public static extern string DSP_MODEL_ATTRIB_TIMESTAMP_get();

	// Token: 0x060012B0 RID: 4784
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_MODEL_ATTRIB_CAPTURE_ID_get")]
	public static extern string DSP_MODEL_ATTRIB_CAPTURE_ID_get();

	// Token: 0x060012B1 RID: 4785
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_MODEL_ATTRIB_METRIC_ID_get")]
	public static extern string DSP_MODEL_ATTRIB_METRIC_ID_get();

	// Token: 0x060012B2 RID: 4786
	[DllImport("SDPCore", EntryPoint = "CSharp_DSP_MODEL_ATTRIB_COUNTER_VALUE_get")]
	public static extern string DSP_MODEL_ATTRIB_COUNTER_VALUE_get();

	// Token: 0x060012B3 RID: 4787
	[DllImport("SDPCore", EntryPoint = "CSharp_SDP_PROCESSOR_PLUGINS_PATH_get")]
	public static extern string SDP_PROCESSOR_PLUGINS_PATH_get();

	// Token: 0x060012B4 RID: 4788
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_ProcessorPluginMgr")]
	public static extern void delete_ProcessorPluginMgr(HandleRef jarg1);

	// Token: 0x060012B5 RID: 4789
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessorPluginMgr_Get")]
	public static extern IntPtr ProcessorPluginMgr_Get();

	// Token: 0x060012B6 RID: 4790
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessorPluginMgr_ShutDown")]
	public static extern void ProcessorPluginMgr_ShutDown(HandleRef jarg1);

	// Token: 0x060012B7 RID: 4791
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessorPluginMgr_ImportCapture")]
	public static extern bool ProcessorPluginMgr_ImportCapture(HandleRef jarg1, int jarg2, int jarg3, string jarg4, int jarg5, int jarg6, int jarg7);

	// Token: 0x060012B8 RID: 4792
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessorPluginMgr_LoadPlugins__SWIG_0")]
	public static extern void ProcessorPluginMgr_LoadPlugins__SWIG_0(HandleRef jarg1, HandleRef jarg2, string jarg3);

	// Token: 0x060012B9 RID: 4793
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessorPluginMgr_LoadPlugins__SWIG_1")]
	public static extern void ProcessorPluginMgr_LoadPlugins__SWIG_1(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060012BA RID: 4794
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessorPluginMgr_UnloadPlugins")]
	public static extern void ProcessorPluginMgr_UnloadPlugins(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060012BB RID: 4795
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessorPluginMgr_GetPlugin")]
	public static extern IntPtr ProcessorPluginMgr_GetPlugin(HandleRef jarg1, string jarg2);

	// Token: 0x060012BC RID: 4796
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_SDPPlugin")]
	public static extern void delete_SDPPlugin(HandleRef jarg1);

	// Token: 0x060012BD RID: 4797
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPPlugin_Initialize__SWIG_0")]
	public static extern bool SDPPlugin_Initialize__SWIG_0(HandleRef jarg1, IntPtr jarg2);

	// Token: 0x060012BE RID: 4798
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPPlugin_Initialize__SWIG_1")]
	public static extern bool SDPPlugin_Initialize__SWIG_1(HandleRef jarg1);

	// Token: 0x060012BF RID: 4799
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPPlugin_Shutdown")]
	public static extern bool SDPPlugin_Shutdown(HandleRef jarg1);

	// Token: 0x060012C0 RID: 4800
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPPlugin_GetName")]
	public static extern string SDPPlugin_GetName(HandleRef jarg1);

	// Token: 0x060012C1 RID: 4801
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPPlugin_GetProviderID")]
	public static extern uint SDPPlugin_GetProviderID(HandleRef jarg1);

	// Token: 0x060012C2 RID: 4802
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPProcessorPlugin_ImportCapture")]
	public static extern bool SDPProcessorPlugin_ImportCapture(HandleRef jarg1, uint jarg2, uint jarg3, string jarg4, int jarg5, int jarg6, int jarg7);

	// Token: 0x060012C3 RID: 4803
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPProcessorPlugin_ProcessData")]
	public static extern void SDPProcessorPlugin_ProcessData(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4, IntPtr jarg5, uint jarg6, Void_Double_Fn jarg7);

	// Token: 0x060012C4 RID: 4804
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPProcessorPlugin_GetLocalBuffer")]
	public static extern IntPtr SDPProcessorPlugin_GetLocalBuffer(HandleRef jarg1, uint jarg2, uint jarg3, uint jarg4);

	// Token: 0x060012C5 RID: 4805
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_SDPProcessorPlugin")]
	public static extern void delete_SDPProcessorPlugin(HandleRef jarg1);

	// Token: 0x060012C6 RID: 4806
	[DllImport("SDPCore", EntryPoint = "CSharp_new_Logger_LogSink")]
	public static extern IntPtr new_Logger_LogSink(int jarg1);

	// Token: 0x060012C7 RID: 4807
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_Logger_LogSink")]
	public static extern void delete_Logger_LogSink(HandleRef jarg1);

	// Token: 0x060012C8 RID: 4808
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_LogSink_GetName")]
	public static extern string Logger_LogSink_GetName(HandleRef jarg1);

	// Token: 0x060012C9 RID: 4809
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_LogSink_Init")]
	public static extern void Logger_LogSink_Init(HandleRef jarg1);

	// Token: 0x060012CA RID: 4810
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_LogSink_InitSwigExplicitLogSink")]
	public static extern void Logger_LogSink_InitSwigExplicitLogSink(HandleRef jarg1);

	// Token: 0x060012CB RID: 4811
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_LogSink_Write")]
	public static extern void Logger_LogSink_Write(HandleRef jarg1, int jarg2, string jarg3, string jarg4);

	// Token: 0x060012CC RID: 4812
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_LogSink_SetLevel")]
	public static extern void Logger_LogSink_SetLevel(HandleRef jarg1, int jarg2);

	// Token: 0x060012CD RID: 4813
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_LogSink_GetLevel")]
	public static extern int Logger_LogSink_GetLevel(HandleRef jarg1);

	// Token: 0x060012CE RID: 4814
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_LogSink_CanLog")]
	public static extern bool Logger_LogSink_CanLog(HandleRef jarg1, int jarg2);

	// Token: 0x060012CF RID: 4815
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_LogSink_director_connect")]
	public static extern void Logger_LogSink_director_connect(HandleRef jarg1, Logger.LogSink.SwigDelegateLogSink_0 delegate0, Logger.LogSink.SwigDelegateLogSink_1 delegate1, Logger.LogSink.SwigDelegateLogSink_2 delegate2);

	// Token: 0x060012D0 RID: 4816
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_Get")]
	public static extern IntPtr Logger_Get();

	// Token: 0x060012D1 RID: 4817
	[DllImport("SDPCore", EntryPoint = "CSharp_new_Logger")]
	public static extern IntPtr new_Logger();

	// Token: 0x060012D2 RID: 4818
	[DllImport("SDPCore", EntryPoint = "CSharp_delete_Logger")]
	public static extern void delete_Logger(HandleRef jarg1);

	// Token: 0x060012D3 RID: 4819
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_SetDefaultTag")]
	public static extern void Logger_SetDefaultTag(HandleRef jarg1, string jarg2);

	// Token: 0x060012D4 RID: 4820
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_GetDefaultTag")]
	public static extern string Logger_GetDefaultTag(HandleRef jarg1);

	// Token: 0x060012D5 RID: 4821
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_AddSink")]
	public static extern void Logger_AddSink(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060012D6 RID: 4822
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_RemoveSink")]
	public static extern void Logger_RemoveSink(HandleRef jarg1, HandleRef jarg2);

	// Token: 0x060012D7 RID: 4823
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_RemoveAllSinks")]
	public static extern void Logger_RemoveAllSinks(HandleRef jarg1);

	// Token: 0x060012D8 RID: 4824
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_GetSinks")]
	public static extern IntPtr Logger_GetSinks(HandleRef jarg1);

	// Token: 0x060012D9 RID: 4825
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_Write__SWIG_0")]
	public static extern void Logger_Write__SWIG_0(HandleRef jarg1, int jarg2, string jarg3, string jarg4);

	// Token: 0x060012DA RID: 4826
	[DllImport("SDPCore", EntryPoint = "CSharp_Logger_WriteFormatted")]
	public static extern void Logger_WriteFormatted(HandleRef jarg1, int jarg2, string jarg3, string jarg4);

	// Token: 0x060012DB RID: 4827
	[DllImport("SDPCore", EntryPoint = "CSharp_StartCapture_SWIGUpcast")]
	public static extern IntPtr StartCapture_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012DC RID: 4828
	[DllImport("SDPCore", EntryPoint = "CSharp_StartCaptureTimeTOD_SWIGUpcast")]
	public static extern IntPtr StartCaptureTimeTOD_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012DD RID: 4829
	[DllImport("SDPCore", EntryPoint = "CSharp_StopCapture_SWIGUpcast")]
	public static extern IntPtr StopCapture_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012DE RID: 4830
	[DllImport("SDPCore", EntryPoint = "CSharp_StopCaptureTimeTOD_SWIGUpcast")]
	public static extern IntPtr StopCaptureTimeTOD_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012DF RID: 4831
	[DllImport("SDPCore", EntryPoint = "CSharp_CancelCapture_SWIGUpcast")]
	public static extern IntPtr CancelCapture_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012E0 RID: 4832
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCaptureComplete_SWIGUpcast")]
	public static extern IntPtr ReportCaptureComplete_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012E1 RID: 4833
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportBufferRegistered_SWIGUpcast")]
	public static extern IntPtr ReportBufferRegistered_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012E2 RID: 4834
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyDataNetAddress_SWIGUpcast")]
	public static extern IntPtr ReplyDataNetAddress_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012E3 RID: 4835
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestDataNetAddress_SWIGUpcast")]
	public static extern IntPtr RequestDataNetAddress_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012E4 RID: 4836
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportDeviceMemoryLow_SWIGUpcast")]
	public static extern IntPtr ReportDeviceMemoryLow_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012E5 RID: 4837
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyServiceLocalityData_SWIGUpcast")]
	public static extern IntPtr ReplyServiceLocalityData_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012E6 RID: 4838
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestDataProviderInfo_SWIGUpcast")]
	public static extern IntPtr RequestDataProviderInfo_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012E7 RID: 4839
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyDataProviderInfo_SWIGUpcast")]
	public static extern IntPtr ReplyDataProviderInfo_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012E8 RID: 4840
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessAdded_SWIGUpcast")]
	public static extern IntPtr ProcessAdded_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012E9 RID: 4841
	[DllImport("SDPCore", EntryPoint = "CSharp_ProcessRemoved_SWIGUpcast")]
	public static extern IntPtr ProcessRemoved_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012EA RID: 4842
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestProcessStatus_SWIGUpcast")]
	public static extern IntPtr RequestProcessStatus_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012EB RID: 4843
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricCategories_SWIGUpcast")]
	public static extern IntPtr RequestMetricCategories_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012EC RID: 4844
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategory_SWIGUpcast")]
	public static extern IntPtr ReplyMetricCategory_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012ED RID: 4845
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategoryTotal_SWIGUpcast")]
	public static extern IntPtr ReplyMetricCategoryTotal_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012EE RID: 4846
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricCategoryAvailable_SWIGUpcast")]
	public static extern IntPtr ReplyMetricCategoryAvailable_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012EF RID: 4847
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetrics_SWIGUpcast")]
	public static extern IntPtr RequestMetrics_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012F0 RID: 4848
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetric_SWIGUpcast")]
	public static extern IntPtr ReplyMetric_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012F1 RID: 4849
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricTotal_SWIGUpcast")]
	public static extern IntPtr ReplyMetricTotal_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012F2 RID: 4850
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricEnable_SWIGUpcast")]
	public static extern IntPtr RequestMetricEnable_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012F3 RID: 4851
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricStatus_SWIGUpcast")]
	public static extern IntPtr RequestMetricStatus_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012F4 RID: 4852
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricStatus_SWIGUpcast")]
	public static extern IntPtr ReplyMetricStatus_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012F5 RID: 4853
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyMetricAvailable_SWIGUpcast")]
	public static extern IntPtr ReplyMetricAvailable_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012F6 RID: 4854
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestMetricHidden_SWIGUpcast")]
	public static extern IntPtr RequestMetricHidden_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012F7 RID: 4855
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestLaunchApp_SWIGUpcast")]
	public static extern IntPtr RequestLaunchApp_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012F8 RID: 4856
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyLaunchApp_SWIGUpcast")]
	public static extern IntPtr ReplyLaunchApp_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012F9 RID: 4857
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestStopApp_SWIGUpcast")]
	public static extern IntPtr RequestStopApp_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012FA RID: 4858
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyStopApp_SWIGUpcast")]
	public static extern IntPtr ReplyStopApp_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012FB RID: 4859
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestCustomData_SWIGUpcast")]
	public static extern IntPtr RequestCustomData_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012FC RID: 4860
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomData_SWIGUpcast")]
	public static extern IntPtr ReportCustomData_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012FD RID: 4861
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataBind_SWIGUpcast")]
	public static extern IntPtr ReportCustomDataBind_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012FE RID: 4862
	[DllImport("SDPCore", EntryPoint = "CSharp_ReportCustomDataAttribute_SWIGUpcast")]
	public static extern IntPtr ReportCustomDataAttribute_SWIGUpcast(IntPtr jarg1);

	// Token: 0x060012FF RID: 4863
	[DllImport("SDPCore", EntryPoint = "CSharp_ICPStringPairMessage_SWIGUpcast")]
	public static extern IntPtr ICPStringPairMessage_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001300 RID: 4864
	[DllImport("SDPCore", EntryPoint = "CSharp_UpdateServiceMsg_SWIGUpcast")]
	public static extern IntPtr UpdateServiceMsg_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001301 RID: 4865
	[DllImport("SDPCore", EntryPoint = "CSharp_MetricManager_SessionEventHandler_SWIGUpcast")]
	public static extern IntPtr MetricManager_SessionEventHandler_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001302 RID: 4866
	[DllImport("SDPCore", EntryPoint = "CSharp_Client_SWIGUpcast")]
	public static extern IntPtr Client_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001303 RID: 4867
	[DllImport("SDPCore", EntryPoint = "CSharp_DataProvider_SWIGUpcast")]
	public static extern IntPtr DataProvider_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001304 RID: 4868
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestOptions_SWIGUpcast")]
	public static extern IntPtr RequestOptions_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001305 RID: 4869
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOption_SWIGUpcast")]
	public static extern IntPtr ReplyOption_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001306 RID: 4870
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionSet_SWIGUpcast")]
	public static extern IntPtr ReplyOptionSet_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001307 RID: 4871
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestOptionReset_SWIGUpcast")]
	public static extern IntPtr RequestOptionReset_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001308 RID: 4872
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionAttribute_SWIGUpcast")]
	public static extern IntPtr ReplyOptionAttribute_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001309 RID: 4873
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionBool_SWIGUpcast")]
	public static extern IntPtr OptionBool_SWIGUpcast(IntPtr jarg1);

	// Token: 0x0600130A RID: 4874
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionString_SWIGUpcast")]
	public static extern IntPtr OptionString_SWIGUpcast(IntPtr jarg1);

	// Token: 0x0600130B RID: 4875
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionInt32_SWIGUpcast")]
	public static extern IntPtr OptionInt32_SWIGUpcast(IntPtr jarg1);

	// Token: 0x0600130C RID: 4876
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionInt64_SWIGUpcast")]
	public static extern IntPtr OptionInt64_SWIGUpcast(IntPtr jarg1);

	// Token: 0x0600130D RID: 4877
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionUInt32_SWIGUpcast")]
	public static extern IntPtr OptionUInt32_SWIGUpcast(IntPtr jarg1);

	// Token: 0x0600130E RID: 4878
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionUInt64_SWIGUpcast")]
	public static extern IntPtr OptionUInt64_SWIGUpcast(IntPtr jarg1);

	// Token: 0x0600130F RID: 4879
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionFloat_SWIGUpcast")]
	public static extern IntPtr OptionFloat_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001310 RID: 4880
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionDouble_SWIGUpcast")]
	public static extern IntPtr OptionDouble_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001311 RID: 4881
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionBinary_SWIGUpcast")]
	public static extern IntPtr OptionBinary_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001312 RID: 4882
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionEnum_SWIGUpcast")]
	public static extern IntPtr OptionEnum_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001313 RID: 4883
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionColor_SWIGUpcast")]
	public static extern IntPtr OptionColor_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001314 RID: 4884
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionStruct_SWIGUpcast")]
	public static extern IntPtr OptionStruct_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001315 RID: 4885
	[DllImport("SDPCore", EntryPoint = "CSharp_RequestOptionCategories_SWIGUpcast")]
	public static extern IntPtr RequestOptionCategories_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001316 RID: 4886
	[DllImport("SDPCore", EntryPoint = "CSharp_ReplyOptionCategory_SWIGUpcast")]
	public static extern IntPtr ReplyOptionCategory_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001317 RID: 4887
	[DllImport("SDPCore", EntryPoint = "CSharp_OptionCategory_SWIGUpcast")]
	public static extern IntPtr OptionCategory_SWIGUpcast(IntPtr jarg1);

	// Token: 0x06001318 RID: 4888
	[DllImport("SDPCore", EntryPoint = "CSharp_SDPProcessorPlugin_SWIGUpcast")]
	public static extern IntPtr SDPProcessorPlugin_SWIGUpcast(IntPtr jarg1);

	// Token: 0x040001A4 RID: 420
	protected static SDPCorePINVOKE.SWIGExceptionHelper swigExceptionHelper = new SDPCorePINVOKE.SWIGExceptionHelper();

	// Token: 0x040001A5 RID: 421
	protected static SDPCorePINVOKE.SWIGStringHelper swigStringHelper = new SDPCorePINVOKE.SWIGStringHelper();

	// Token: 0x020000F3 RID: 243
	protected class SWIGExceptionHelper
	{
		// Token: 0x0600153B RID: 5435
		[DllImport("SDPCore")]
		public static extern void SWIGRegisterExceptionCallbacks_SDPCore(SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate applicationDelegate, SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate arithmeticDelegate, SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate divideByZeroDelegate, SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate indexOutOfRangeDelegate, SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidCastDelegate, SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidOperationDelegate, SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate ioDelegate, SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate nullReferenceDelegate, SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate outOfMemoryDelegate, SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate overflowDelegate, SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate systemExceptionDelegate);

		// Token: 0x0600153C RID: 5436
		[DllImport("SDPCore", EntryPoint = "SWIGRegisterExceptionArgumentCallbacks_SDPCore")]
		public static extern void SWIGRegisterExceptionCallbacksArgument_SDPCore(SDPCorePINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentDelegate, SDPCorePINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentNullDelegate, SDPCorePINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentOutOfRangeDelegate);

		// Token: 0x0600153D RID: 5437 RVA: 0x0001A535 File Offset: 0x00018735
		private static void SetPendingApplicationException(string message)
		{
			SDPCorePINVOKE.SWIGPendingException.Set(new ApplicationException(message, SDPCorePINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x0600153E RID: 5438 RVA: 0x0001A547 File Offset: 0x00018747
		private static void SetPendingArithmeticException(string message)
		{
			SDPCorePINVOKE.SWIGPendingException.Set(new ArithmeticException(message, SDPCorePINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x0600153F RID: 5439 RVA: 0x0001A559 File Offset: 0x00018759
		private static void SetPendingDivideByZeroException(string message)
		{
			SDPCorePINVOKE.SWIGPendingException.Set(new DivideByZeroException(message, SDPCorePINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x06001540 RID: 5440 RVA: 0x0001A56B File Offset: 0x0001876B
		private static void SetPendingIndexOutOfRangeException(string message)
		{
			SDPCorePINVOKE.SWIGPendingException.Set(new IndexOutOfRangeException(message, SDPCorePINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x06001541 RID: 5441 RVA: 0x0001A57D File Offset: 0x0001877D
		private static void SetPendingInvalidCastException(string message)
		{
			SDPCorePINVOKE.SWIGPendingException.Set(new InvalidCastException(message, SDPCorePINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x0001A58F File Offset: 0x0001878F
		private static void SetPendingInvalidOperationException(string message)
		{
			SDPCorePINVOKE.SWIGPendingException.Set(new InvalidOperationException(message, SDPCorePINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x0001A5A1 File Offset: 0x000187A1
		private static void SetPendingIOException(string message)
		{
			SDPCorePINVOKE.SWIGPendingException.Set(new IOException(message, SDPCorePINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x0001A5B3 File Offset: 0x000187B3
		private static void SetPendingNullReferenceException(string message)
		{
			SDPCorePINVOKE.SWIGPendingException.Set(new NullReferenceException(message, SDPCorePINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x06001545 RID: 5445 RVA: 0x0001A5C5 File Offset: 0x000187C5
		private static void SetPendingOutOfMemoryException(string message)
		{
			SDPCorePINVOKE.SWIGPendingException.Set(new OutOfMemoryException(message, SDPCorePINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x06001546 RID: 5446 RVA: 0x0001A5D7 File Offset: 0x000187D7
		private static void SetPendingOverflowException(string message)
		{
			SDPCorePINVOKE.SWIGPendingException.Set(new OverflowException(message, SDPCorePINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x06001547 RID: 5447 RVA: 0x0001A5E9 File Offset: 0x000187E9
		private static void SetPendingSystemException(string message)
		{
			SDPCorePINVOKE.SWIGPendingException.Set(new SystemException(message, SDPCorePINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x06001548 RID: 5448 RVA: 0x0001A5FB File Offset: 0x000187FB
		private static void SetPendingArgumentException(string message, string paramName)
		{
			SDPCorePINVOKE.SWIGPendingException.Set(new ArgumentException(message, paramName, SDPCorePINVOKE.SWIGPendingException.Retrieve()));
		}

		// Token: 0x06001549 RID: 5449 RVA: 0x0001A610 File Offset: 0x00018810
		private static void SetPendingArgumentNullException(string message, string paramName)
		{
			Exception ex = SDPCorePINVOKE.SWIGPendingException.Retrieve();
			if (ex != null)
			{
				message = message + " Inner Exception: " + ex.Message;
			}
			SDPCorePINVOKE.SWIGPendingException.Set(new ArgumentNullException(paramName, message));
		}

		// Token: 0x0600154A RID: 5450 RVA: 0x0001A648 File Offset: 0x00018848
		private static void SetPendingArgumentOutOfRangeException(string message, string paramName)
		{
			Exception ex = SDPCorePINVOKE.SWIGPendingException.Retrieve();
			if (ex != null)
			{
				message = message + " Inner Exception: " + ex.Message;
			}
			SDPCorePINVOKE.SWIGPendingException.Set(new ArgumentOutOfRangeException(paramName, message));
		}

		// Token: 0x0600154B RID: 5451 RVA: 0x0001A680 File Offset: 0x00018880
		static SWIGExceptionHelper()
		{
			SDPCorePINVOKE.SWIGExceptionHelper.SWIGRegisterExceptionCallbacks_SDPCore(SDPCorePINVOKE.SWIGExceptionHelper.applicationDelegate, SDPCorePINVOKE.SWIGExceptionHelper.arithmeticDelegate, SDPCorePINVOKE.SWIGExceptionHelper.divideByZeroDelegate, SDPCorePINVOKE.SWIGExceptionHelper.indexOutOfRangeDelegate, SDPCorePINVOKE.SWIGExceptionHelper.invalidCastDelegate, SDPCorePINVOKE.SWIGExceptionHelper.invalidOperationDelegate, SDPCorePINVOKE.SWIGExceptionHelper.ioDelegate, SDPCorePINVOKE.SWIGExceptionHelper.nullReferenceDelegate, SDPCorePINVOKE.SWIGExceptionHelper.outOfMemoryDelegate, SDPCorePINVOKE.SWIGExceptionHelper.overflowDelegate, SDPCorePINVOKE.SWIGExceptionHelper.systemDelegate);
			SDPCorePINVOKE.SWIGExceptionHelper.SWIGRegisterExceptionCallbacksArgument_SDPCore(SDPCorePINVOKE.SWIGExceptionHelper.argumentDelegate, SDPCorePINVOKE.SWIGExceptionHelper.argumentNullDelegate, SDPCorePINVOKE.SWIGExceptionHelper.argumentOutOfRangeDelegate);
		}

		// Token: 0x0400021D RID: 541
		private static SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate applicationDelegate = new SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate(SDPCorePINVOKE.SWIGExceptionHelper.SetPendingApplicationException);

		// Token: 0x0400021E RID: 542
		private static SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate arithmeticDelegate = new SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate(SDPCorePINVOKE.SWIGExceptionHelper.SetPendingArithmeticException);

		// Token: 0x0400021F RID: 543
		private static SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate divideByZeroDelegate = new SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate(SDPCorePINVOKE.SWIGExceptionHelper.SetPendingDivideByZeroException);

		// Token: 0x04000220 RID: 544
		private static SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate indexOutOfRangeDelegate = new SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate(SDPCorePINVOKE.SWIGExceptionHelper.SetPendingIndexOutOfRangeException);

		// Token: 0x04000221 RID: 545
		private static SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidCastDelegate = new SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate(SDPCorePINVOKE.SWIGExceptionHelper.SetPendingInvalidCastException);

		// Token: 0x04000222 RID: 546
		private static SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate invalidOperationDelegate = new SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate(SDPCorePINVOKE.SWIGExceptionHelper.SetPendingInvalidOperationException);

		// Token: 0x04000223 RID: 547
		private static SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate ioDelegate = new SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate(SDPCorePINVOKE.SWIGExceptionHelper.SetPendingIOException);

		// Token: 0x04000224 RID: 548
		private static SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate nullReferenceDelegate = new SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate(SDPCorePINVOKE.SWIGExceptionHelper.SetPendingNullReferenceException);

		// Token: 0x04000225 RID: 549
		private static SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate outOfMemoryDelegate = new SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate(SDPCorePINVOKE.SWIGExceptionHelper.SetPendingOutOfMemoryException);

		// Token: 0x04000226 RID: 550
		private static SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate overflowDelegate = new SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate(SDPCorePINVOKE.SWIGExceptionHelper.SetPendingOverflowException);

		// Token: 0x04000227 RID: 551
		private static SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate systemDelegate = new SDPCorePINVOKE.SWIGExceptionHelper.ExceptionDelegate(SDPCorePINVOKE.SWIGExceptionHelper.SetPendingSystemException);

		// Token: 0x04000228 RID: 552
		private static SDPCorePINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentDelegate = new SDPCorePINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate(SDPCorePINVOKE.SWIGExceptionHelper.SetPendingArgumentException);

		// Token: 0x04000229 RID: 553
		private static SDPCorePINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentNullDelegate = new SDPCorePINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate(SDPCorePINVOKE.SWIGExceptionHelper.SetPendingArgumentNullException);

		// Token: 0x0400022A RID: 554
		private static SDPCorePINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate argumentOutOfRangeDelegate = new SDPCorePINVOKE.SWIGExceptionHelper.ExceptionArgumentDelegate(SDPCorePINVOKE.SWIGExceptionHelper.SetPendingArgumentOutOfRangeException);

		// Token: 0x020000FB RID: 251
		// (Invoke) Token: 0x0600156E RID: 5486
		public delegate void ExceptionDelegate(string message);

		// Token: 0x020000FC RID: 252
		// (Invoke) Token: 0x06001572 RID: 5490
		public delegate void ExceptionArgumentDelegate(string message, string paramName);
	}

	// Token: 0x020000F4 RID: 244
	public class SWIGPendingException
	{
		// Token: 0x17000315 RID: 789
		// (get) Token: 0x0600154D RID: 5453 RVA: 0x0001A7CC File Offset: 0x000189CC
		public static bool Pending
		{
			get
			{
				bool flag = false;
				if (SDPCorePINVOKE.SWIGPendingException.numExceptionsPending > 0 && SDPCorePINVOKE.SWIGPendingException.pendingException != null)
				{
					flag = true;
				}
				return flag;
			}
		}

		// Token: 0x0600154E RID: 5454 RVA: 0x0001A7F0 File Offset: 0x000189F0
		public static void Set(Exception e)
		{
			if (SDPCorePINVOKE.SWIGPendingException.pendingException != null)
			{
				throw new ApplicationException("FATAL: An earlier pending exception from unmanaged code was missed and thus not thrown (" + SDPCorePINVOKE.SWIGPendingException.pendingException.ToString() + ")", e);
			}
			SDPCorePINVOKE.SWIGPendingException.pendingException = e;
			Type typeFromHandle = typeof(SDPCorePINVOKE);
			lock (typeFromHandle)
			{
				SDPCorePINVOKE.SWIGPendingException.numExceptionsPending++;
			}
		}

		// Token: 0x0600154F RID: 5455 RVA: 0x0001A868 File Offset: 0x00018A68
		public static Exception Retrieve()
		{
			Exception ex = null;
			if (SDPCorePINVOKE.SWIGPendingException.numExceptionsPending > 0 && SDPCorePINVOKE.SWIGPendingException.pendingException != null)
			{
				ex = SDPCorePINVOKE.SWIGPendingException.pendingException;
				SDPCorePINVOKE.SWIGPendingException.pendingException = null;
				Type typeFromHandle = typeof(SDPCorePINVOKE);
				lock (typeFromHandle)
				{
					SDPCorePINVOKE.SWIGPendingException.numExceptionsPending--;
				}
			}
			return ex;
		}

		// Token: 0x0400022B RID: 555
		[ThreadStatic]
		private static Exception pendingException;

		// Token: 0x0400022C RID: 556
		private static int numExceptionsPending;
	}

	// Token: 0x020000F5 RID: 245
	protected class SWIGStringHelper
	{
		// Token: 0x06001551 RID: 5457
		[DllImport("SDPCore")]
		public static extern void SWIGRegisterStringCallback_SDPCore(SDPCorePINVOKE.SWIGStringHelper.SWIGStringDelegate stringDelegate);

		// Token: 0x06001552 RID: 5458 RVA: 0x0001A8D0 File Offset: 0x00018AD0
		private static string CreateString(string cString)
		{
			return cString;
		}

		// Token: 0x06001553 RID: 5459 RVA: 0x0001A8D3 File Offset: 0x00018AD3
		static SWIGStringHelper()
		{
			SDPCorePINVOKE.SWIGStringHelper.SWIGRegisterStringCallback_SDPCore(SDPCorePINVOKE.SWIGStringHelper.stringDelegate);
		}

		// Token: 0x0400022D RID: 557
		private static SDPCorePINVOKE.SWIGStringHelper.SWIGStringDelegate stringDelegate = new SDPCorePINVOKE.SWIGStringHelper.SWIGStringDelegate(SDPCorePINVOKE.SWIGStringHelper.CreateString);

		// Token: 0x020000FD RID: 253
		// (Invoke) Token: 0x06001576 RID: 5494
		public delegate string SWIGStringDelegate(string message);
	}
}
