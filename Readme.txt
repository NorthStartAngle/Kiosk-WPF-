-Binding self`s parent containter with relativesource
Width="{Binding ActualWidth, RelativeSource = {RelativeSource AncestorType = {x:Type Page}}}" 
-Binding Self:
	<Rectangle Fill="Red" Name="rectangle"
                    Height="100" Stroke="Black"
                    Canvas.Top="100" Canvas.Left="100"
                    Width="{Binding ElementName=rectangle,
                    Path=Height}"/>

	same above example
	<Rectangle Fill="Red" Height="100"
                   Stroke="Black"
                   Width="{Binding RelativeSource={RelativeSource Self},
                   Path=Height}"/>
- Binding FindAncestor
	Text="{Binding RelativeSource={RelativeSource 
                           FindAncestor,
                           AncestorType={x:Type Border},
                           AncestorLevel=2},Path=Name}"
                           Width="200"/>
	Over 2 level, type is Border