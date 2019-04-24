module Fable.ReactGoogleMaps

open Fable.Core
open Fable.Import
open Fable.Core.JsInterop
open Fable.React

type MapRef(mapRef) =

    /// Get the current bounds of the Map
    member __.GetBounds() : LatLngBounds option =
        if isNull mapRef then
            None
        else
            Some(mapRef?getBounds() |> unbox)

    member __.GetZoom() : int option =
        if isNull mapRef then
            None
        else
            Some(mapRef?getZoom() |> unbox)

    member __.GetCenter() : LatLng option =
        if isNull mapRef then
            None
        else
            Some(mapRef?getCenter() |> unbox)

    member __.FitBounds(bounds: LatLngBounds, ?padding: float) : unit =
        if not(isNull mapRef) then
            mapRef?fitBounds(bounds, padding) |> ignore

module Props =

    type IInfoWindowProperties =
        interface end

    [<RequireQualifiedAccess>]
    type InfoWindowProperties =
    | OnCloseClick of (unit -> unit)
        interface IInfoWindowProperties

    type ISearchBoxProperties =
        interface end

    [<RequireQualifiedAccess>]
    type SearchBoxProperties =
    | Ref of (obj -> unit)
    | OnPlacesChanged of (unit -> unit)
        interface ISearchBoxProperties

    type IMarkerProperties =
        interface end

    [<RequireQualifiedAccess>]
    type MarkerProperties =
    | Key of obj
    | Title of string
    | Icon of string
    | Label of obj
    | OnClick of (unit -> unit)
    | Position of U2<LatLng, LatLngLiteral>
        interface IMarkerProperties

    type IMarkerClustererProperties =
        interface end

    [<RequireQualifiedAccess>]
    type MarkerClustererProperties =
    | Key of obj
    | AverageCenter of bool
    | MaxZoom of int
    | EnableRetinaIcons of bool
    | GridSize of float
        interface IMarkerClustererProperties

    type IMapProperties =
        interface end

    [<RequireQualifiedAccess>]
    type MapProperties =
    | OnMapMounted of (obj -> unit)
    | ApiKey of string
    | DefaultZoom of int
    | SearchBoxText of string
    | ShowSearchBox of bool
    | ShowTrafficLayer of bool
    | DefaultCenter of U2<LatLng,LatLngLiteral>
    | Center of U2<LatLng, LatLngLiteral>
    | OnCenterChanged of (unit -> unit)
    | OnPlacesChanged of (Place [] -> unit)
    | OnZoomChanged of (unit -> unit)
    | OnIdle of (unit -> unit)
    | OnClick of (MouseEvent -> unit)
    | Markers of React.ReactElement seq
    | [<CompiledName("Markers")>] Clusterer of React.ReactElement
    | MapLoadingContainer of string
    | MapContainer of string
    | Options of obj
        interface IMapProperties

/// A wrapper around google.maps.InfoWindow
let infoWindow (props:Props.IInfoWindowProperties list) child : React.ReactElement =
    ofImport "InfoWindow" "react-google-maps" (keyValueList CaseRules.LowerFirst props) [child]
 
/// A wrapper around google.maps.places.SearchBox on the map
let searchBox (props:Props.ISearchBoxProperties list) children : React.ReactElement =
    ofImport "SearchBox" "react-google-maps" (keyValueList CaseRules.LowerFirst props) children

/// A wrapper around google.maps.Marker
let marker (props:Props.IMarkerProperties list) children : React.ReactElement =
    ofImport "Marker" "react-google-maps" (keyValueList CaseRules.LowerFirst props) children

/// A wrapper around MarkerClusterer - https://github.com/mahnunchik/markerclustererplus/blob/master/docs/reference.html
let markerClusterer (props:Props.IMarkerClustererProperties list) (markers:React.ReactElement list) : React.ReactElement =
    ofImport "MarkerClusterer" "react-google-maps/lib/components/addons/MarkerClusterer.js" (keyValueList CaseRules.LowerFirst props) markers

/// A wrapper around google.maps.Map
let googleMap (props:Props.IMapProperties list) : React.ReactElement =
    ofImport "GoogleMapComponent" "./mapComponent.js" (keyValueList CaseRules.LowerFirst props) []


let getPosition (options: obj) : Fable.Core.JS.Promise<Fable.Import.Browser.Position> = importMember "./location.js"