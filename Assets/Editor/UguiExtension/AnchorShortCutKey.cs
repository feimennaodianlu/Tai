using UnityEditor;
using UnityEngine;
public class AnchorShortCutKey
{
	static void AdjustAnchor(float offsetMinX, float offsetMinY, float offsetMaxX, float offsetMaxY)
	{
		//记录对象的状态，为了以后执行撤消时恢复状态
		Undo.RecordObjects( Selection.transforms, "AdjustAnchor" );
		foreach( var transform in Selection.transforms )
		{
			RectTransform t = transform as RectTransform;
			RectTransform parentTransform = Selection.activeTransform.parent as RectTransform;
			if( t == null || parentTransform == null )
				return;
			var distanceMinX = t.rect.width * offsetMinX;
			var distanceMinY = t.rect.height * offsetMinY;
			var distanceMaxX = t.rect.width * offsetMaxX;
			var distanceMaxY = t.rect.height * offsetMaxY;
			var newAnchorMinX = t.anchorMin.x + ( t.offsetMin.x + distanceMinX ) / parentTransform.rect.width;
			var newAnchorMinY = t.anchorMin.y + ( t.offsetMin.y + distanceMinY ) / parentTransform.rect.height;
			var newAnchorMaxX = t.anchorMax.x + ( t.offsetMax.x - distanceMaxX ) / parentTransform.rect.width;
			var newAnchorMaxY = t.anchorMax.y + ( t.offsetMax.y - distanceMaxY ) / parentTransform.rect.height;
			t.anchorMin = new Vector2( newAnchorMinX, newAnchorMinY );
			t.anchorMax = new Vector2( newAnchorMaxX, newAnchorMaxY );
			t.offsetMin = new Vector2( -distanceMinX, -distanceMinY );
			t.offsetMax = new Vector2( distanceMaxX, distanceMaxY );
		}
	}
	[MenuItem( "Wa/RectAnchor/CornersToAnchors &[" )]
	static void CornersToAnchors()
	{
		Undo.RecordObjects( Selection.transforms, "CornersToAnchors" );
		foreach( var transform in Selection.transforms )
		{
			RectTransform t = transform as RectTransform;
			if( t == null )
				return;
			t.offsetMin = t.offsetMax = new Vector2( 0, 0 );
		}
	}
	[MenuItem( "Wa/RectAnchor/AnchorsToCorners &]" )]
	static void AnchorsToCorners()
	{
		AdjustAnchor( 0, 0, 0, 0 );
	}
	[MenuItem( "Wa/RectAnchor/AnchorToMiddleCenter &g" )]
	static void AnchorToMiddleCenter()
	{
		AdjustAnchor( 0.5f, 0.5f, 0.5f, 0.5f );
	}
	[MenuItem( "Wa/RectAnchor/AnchorToMiddleLeft &f" )]
	static void AnchorToMiddleLeft()
	{
		AdjustAnchor( 0, 0.5f, 1f, 0.5f );
	}
	[MenuItem( "Wa/RectAnchor/AnchorToMiddleRight &h" )]
	static void AnchorToMiddleRight()
	{
		AdjustAnchor( 1f, 0.5f, 0, 0.5f );
	}
	[MenuItem( "Wa/RectAnchor/AnchorToTopLeft &r" )]
	static void AnchorToTopLeft()
	{
		AdjustAnchor( 0, 1f, 1f, 0 );
	}
	[MenuItem( "Wa/RectAnchor/AnchorToTopMiddle &t" )]
	static void AnchorToTopMiddle()
	{
		AdjustAnchor( 0.5f, 1f, 0.5f, 0 );
	}
	[MenuItem( "Wa/RectAnchor/AnchorToTopRight &y" )]
	static void AnchorToTopRight()
	{
		AdjustAnchor( 1f, 1f, 0, 0 );
	}
	[MenuItem( "Wa/RectAnchor/AnchorToBottomLeft &v" )]
	static void AnchorToBottomLeft()
	{
		AdjustAnchor( 0, 0, 1f, 1f );
	}
	[MenuItem( "Wa/RectAnchor/AnchorToBottomCenter &b" )]
	static void AnchorToBottomCenter()
	{
		AdjustAnchor( 0.5f, 0, 0.5f, 1f );
	}
	[MenuItem( "Wa/RectAnchor/AnchorToBottomRight &n" )]
	static void AnchorToBottomRight()
	{
		AdjustAnchor( 1f, 0, 0, 1f );
	}
	[MenuItem( "Wa/RectAnchor/AnchorToStretchTop &u" )]
	static void AnchorToStretchTop()
	{
		AdjustAnchor( 0, 1f, 0, 0 );
	}
	[MenuItem( "Wa/RectAnchor/AnchorToStretchMiddle &j" )]
	static void AnchorToStretchMiddle()
	{
		AdjustAnchor( 0, 0.5f, 0, 0.5f );
	}
	[MenuItem( "Wa/RectAnchor/AnchorToStretchButtom &m" )]
	static void AnchorToStretchButtom()
	{
		AdjustAnchor( 0, 0, 0, 1f );
	}
	[MenuItem( "Wa/RectAnchor/AnchorToStretchLeft &i" )]
	static void AnchorToStretchLeft()
	{
		AdjustAnchor( 0, 0, 1f, 0 );
	}
	[MenuItem( "Wa/RectAnchor/AnchorToStretchCenter &o" )]
	static void AnchorToStretchCenter()
	{
		AdjustAnchor( 0.5f, 0, 0.5f, 0 );
	}
	[MenuItem( "Wa/RectAnchor/AnchorToStretchRight &p" )]
	static void AnchorToStretchRight()
	{
		AdjustAnchor( 1f, 0, 0, 0 );
	}
}
